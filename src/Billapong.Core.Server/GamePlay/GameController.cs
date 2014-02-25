namespace Billapong.Core.Server.GamePlay
{
    using Contract.Exceptions;
    using Contract.Service;
    using Converter.Map;
    using DataAccess.Model.Map;
    using DataAccess.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Game controller for handling the gameplay
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Dictionary to store all open and playing games
        /// </summary>
        private readonly IDictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        /// <summary>
        /// The number of rounds
        /// </summary>
        private readonly int numberOfTotalRounds;
        
        #region Singleton Implementation

        /// <summary>
        /// Initializes static members of the <see cref="GameController"/> class.
        /// </summary>
        static GameController()
        {
            Current = new GameController();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GameController"/> class from being created.
        /// </summary>
        private GameController()
        {
            this.unitOfWork = new UnitOfWork();

            // set the number of rounds to play
            if (!int.TryParse(ConfigurationManager.AppSettings["GamePlay.NumberOfRounds"], out this.numberOfTotalRounds))
            {
                this.numberOfTotalRounds = 10;
            }

            // set number of total rounds * 2, because both players play their rounds and each incerement always by 1
            this.numberOfTotalRounds = this.numberOfTotalRounds*2;
        }

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static GameController Current { get; private set; }

        #endregion

        /// <summary>
        /// Opens a new game.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>The id (guid) of the new game</returns>
        public Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username, IGameConsoleCallback callback)
        {
            // load the map
            var map = this.unitOfWork.MapRepository.GetById(mapId);
            if (map == null)
            {
                throw new FaultException<MapNotFoundException>(new MapNotFoundException(mapId), "Map not found");
            }
            
            // generate game
            var game = new Game();
            game.Id = Guid.NewGuid();
            game.Map = map;

            // create the player
            var player = new Player();
            player.Name = username;
            player.VisibleWindows.AddRange(visibleWindows);
            player.Callback = callback;
            
            // set the player as player 1 and start the game
            game.Players[0] = player;

            lock (LockObject)
            {
                this.games.Add(game.Id, game);
            }

            return game.Id;
        }

        /// <summary>
        /// Gets the open games.
        /// </summary>
        /// <returns>All games with status "open"</returns>
        public IEnumerable<Game> GetOpenGames()
        {
            return this.GetAllGames().Where(game => game.Status == GameStatus.Open);
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns>All games, converted to a list</returns>
        public IEnumerable<Game> GetAllGames()
        {
            lock (LockObject)
            {
                return this.games.Values.ToList();
            }
        }

        /// <summary>
        /// Joins an existing game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="callback">The callback.</param>
        public void JoinGame(Guid gameId, string username, IGameConsoleCallback callback)
        {
            Game game = null;
            lock (LockObject)
            {
                if (!this.games.ContainsKey(gameId))
                {
                    throw new FaultException<GameNotFoundException>(new GameNotFoundException(gameId), "Game not found");
                }

                game = this.games[gameId];
                if (game.Status != GameStatus.Open)
                {
                    throw new FaultException<GameNotOpenException>(new GameNotOpenException(gameId), "Game is not is opening state");
                }

                game.Status = GameStatus.Playing;
            }

            // get visible windows from player 1
            var player1VisibleWindows = game.Players[0].VisibleWindows;

            // create second player
            var player = new Player();
            player.Name = username;
            player.Callback = callback;
            player.VisibleWindows.AddRange(game.Map.Windows.Count > 1
                ? game.Map.Windows.Select(window => window.Id).Where(id => !player1VisibleWindows.Contains(id))
                : player1VisibleWindows);

            game.Players[1] = player;

            // send startgame callback
            Task.Run(() => this.StartGameCallback(game));
        }

        /// <summary>
        /// Cancels the game with a specific id.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        public void CancelGame(Guid gameId)
        {
            Game game = null;
            lock (LockObject)
            {
                if (!this.games.ContainsKey(gameId))
                {
                    throw new FaultException<GameNotFoundException>(new GameNotFoundException(gameId), "Game not found");
                }

                game = this.games[gameId];
                game.Status = GameStatus.Canceled;
            }

            Task.Run(() => this.CancelGameCallback(game));
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        public void SetStartPoint(Guid gameId, long windowId, double pointX, double pointY)
        {
            var game = this.GetGame(gameId);
            Task.Run(() => this.SetStartPointCallback(game, windowId, pointX, pointY));
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="directionX">The direction x.</param>
        /// <param name="directionY">The direction y.</param>
        public void StartRound(Guid gameId, double directionX, double directionY)
        {
            var game = this.GetGame(gameId);
            Task.Run(() => this.StartRoundCallback(game, directionX, directionY));
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isPlayer1">if set to <c>true</c> the player who played this round was player 1.</param>
        /// <param name="score">The score.</param>
        public void EndRound(Guid gameId, bool isPlayer1, int score)
        {
            var game = this.GetGame(gameId);

            // increment the played round by 1
            game.BounceCount++;

            // get current player
            var player = game.Players[isPlayer1 ? 0 : 1];
            player.Score += score;

            // is last round?
            var wasLastRound = game.BounceCount >= this.numberOfTotalRounds;
            if (wasLastRound)
            {
                this.RemoveGame(gameId);
                this.SaveHighScore(game);
            }

            // send the callback
            Task.Run(() => this.EndRoundCallback(game, player.Score, wasLastRound));
        }

        /// <summary>
        /// Adds the high score to the database.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        public void AddHighScore(long mapId, string playerName, int score)
        {
            // load the map
            var map = this.unitOfWork.MapRepository.GetById(mapId);
            if (map == null)
            {
                throw new FaultException<MapNotFoundException>(new MapNotFoundException(mapId), "Map not found");
            }

            var highscore = new HighScore
            {
                Map = map,
                PlayerName = playerName,
                Score = score
            };

            lock (LockObject)
            {
                this.unitOfWork.HighScoreRepository.Add(highscore);
                this.unitOfWork.Save();
            }
        }

        private void SaveHighScore(Game game)
        {
            
            foreach (var player in game.Players)
            {
                var highScore = new HighScore();
                highScore.Map = game.Map;
                highScore.PlayerName = player.Name;
                highScore.Score = player.Score;

                lock (LockObject)
                {
                    this.unitOfWork.HighScoreRepository.Add(highScore);
                    this.unitOfWork.Save();
                }
            }
        }

        /// <summary>
        /// Starts the game and send a callback to both players that the game now starts.
        /// </summary>
        /// <param name="game">The game.</param>
        private void StartGameCallback(Game game)
        {
            // evaluate if player one would start
            var player1Start = (new Random()).Next(0, 2) == 0;

            // check callbacks
            if (!this.CheckCallbacks(game))
            {
                this.HandleGameError(game);
                return;
            }

            // get players
            var player1 = game.Players[0];
            var player2 = game.Players[1];

            // send callbacks
            player1.Callback.StartGame(game.Id, game.Map.ToContract(), player2.Name, player1.VisibleWindows, player1Start);
            player2.Callback.StartGame(game.Id, game.Map.ToContract(), player1.Name, player2.VisibleWindows, !player1Start);
        }

        /// <summary>
        /// Cancels the game with sending callback to all clients and removing the game at the end.
        /// </summary>
        /// <param name="game">The game.</param>
        private void CancelGameCallback(Game game)
        {
            // check callbacks
            if (!this.CheckCallbacks(game))
            {
                this.HandleGameError(game);
                return;
            }

            // send callback
            foreach (var player in game.Players)
            {
                if (player != null)
                {
                    player.Callback.CancelGame();
                }
            }

            // remove the game from collection
            this.RemoveGame(game.Id);
        }

        /// <summary>
        /// Sends the callback for setting the start point on the game panel.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        private void SetStartPointCallback(Game game, long windowId, double pointX, double pointY)
        {
            // check callbacks
            if (!this.CheckCallbacks(game))
            {
                this.HandleGameError(game);
                return;
            }

            // send callback
            game.Players[0].Callback.SetStartPoint(windowId, pointX, pointY);
            game.Players[1].Callback.SetStartPoint(windowId, pointX, pointY);
        }

        /// <summary>
        /// SSends the callbacks for starting a round.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        private void StartRoundCallback(Game game, double pointX, double pointY)
        {
            // check callbacks
            if (!this.CheckCallbacks(game))
            {
                this.HandleGameError(game);
                return;
            }

            // send callback
            game.Players[0].Callback.StartRound(pointX, pointY);
            game.Players[1].Callback.StartRound(pointX, pointY);
        }

        /// <summary>
        /// Send callback for ending a round
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="score">The score.</param>
        /// <param name="wasFinalRound">if set to <c>true</c> this was the final round and the game should be finished.</param>
        private void EndRoundCallback(Game game, int score, bool wasFinalRound)
        {
            // check callbacks
            if (!this.CheckCallbacks(game))
            {
                this.HandleGameError(game);
                return;
            }

            // send callback
            game.Players[0].Callback.EndRound(score, wasFinalRound);
            game.Players[1].Callback.EndRound(score, wasFinalRound);
        }

        /// <summary>
        /// Handles a game error and sends a callback to all still connected clients.
        /// </summary>
        /// <param name="game">The game.</param>
        private void HandleGameError(Game game)
        {
            game.Status = GameStatus.Canceled;
            foreach (var player in game.Players)
            {
                var callback = player.Callback;
                if (((ICommunicationObject)callback).State != CommunicationState.Opened)
                {
                    callback.GameError();
                }
            }

            // remote the game from collection
            this.RemoveGame(game.Id);
        }

        /// <summary>
        /// Removes the game from the game collection.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        private void RemoveGame(Guid gameId)
        {
            lock (LockObject)
            {
                this.games.Remove(gameId);
            }
        }

        /// <summary>
        /// Checks the callbacks for the game instance.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>true if ever callback is open, false if one of them is broken</returns>
        private bool CheckCallbacks(Game game)
        {
            foreach (var player in game.Players)
            {
                if (player == null) continue;
                var callback = player.Callback;
                if (callback == null || ((ICommunicationObject) callback).State != CommunicationState.Opened)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{GameNotFoundException}">Game not found</exception>
        private Game GetGame(Guid gameId)
        {
            lock (LockObject)
            {
                if (!this.games.ContainsKey(gameId))
                {
                    throw new FaultException<GameNotFoundException>(new GameNotFoundException(gameId), "Game not found");
                }

                return this.games[gameId];
            }
        }
    }
}
