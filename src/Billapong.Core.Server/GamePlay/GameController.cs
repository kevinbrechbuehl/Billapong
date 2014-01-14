namespace Billapong.Core.Server.GamePlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Contract.Exceptions;
    using Contract.Service;
    using DataAccess.Model.Map;
    using DataAccess.Repository;

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
        /// The repository
        /// </summary>
        private readonly IRepository<Map> mapRepository;

        /// <summary>
        /// Dictionary to store all open and playing games
        /// </summary>
        private readonly IDictionary<Guid, Game> games = new Dictionary<Guid, Game>();
        
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
            this.mapRepository = new Repository<Map>();
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
        /// <exception cref="MapNotFoundException">Map was not found on the server</exception>
        public Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username, IGameConsoleCallback callback)
        {
            // load the map
            var map = this.mapRepository.GetById(mapId);
            if (map == null)
            {
                throw new MapNotFoundException(mapId);
            }
            
            // generate game
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Map = map,
                Player1Name = username
            };

            game.Player1VisibleWindows.AddRange(visibleWindows);
            game.Callbacks.Add(callback);

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
        /// Joins an existing game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="GameNotFoundException">The game was not found on the server</exception>
        /// <exception cref="GameNotOpenException">The game is not open</exception>
        public void JoinGame(Guid gameId, string username, IGameConsoleCallback callback)
        {
            Game game = null;
            lock (LockObject)
            {
                if (!this.games.ContainsKey(gameId))
                {
                    throw new GameNotFoundException(gameId);
                }

                game = this.games[gameId];
                if (game.Status != GameStatus.Open)
                {
                    throw new GameNotOpenException(gameId);
                }

                game.Status = GameStatus.Playing;
            }

            game.Player2Name = username;
            game.Callbacks.Add(callback);
            Task.Run(() => this.StartGame(game));
        }

        /// <summary>
        /// Starts the game and send a callback to both players that the game now starts.
        /// </summary>
        /// <param name="game">The game.</param>
        private void StartGame(Game game)
        {
            // evaluate if player one would start
            var player1Start = (new Random()).Next(0, 2) == 0;
            
            // calculate the visible windows for player 2
            game.Player2VisibleWindows.AddRange(game.Map.Windows.Count > 1
                ? game.Map.Windows.Select(map => map.Id).Where(id => !game.Player1VisibleWindows.Contains(id))
                : game.Player1VisibleWindows);

            // send callback to player 1
            var player1Callback = game.Callbacks.FirstOrDefault();
            if (player1Callback == null || ((ICommunicationObject)player1Callback).State != CommunicationState.Opened)
            {
                game.Callbacks.Remove(player1Callback);
                this.HandleGameError(game);
                return; 
            }

            player1Callback.StartGame(game.Id, game.Player2Name, game.Player1VisibleWindows, player1Start);

            // send callback to player 2
            var player2Callback = game.Callbacks.Skip(1).FirstOrDefault();
            if (player2Callback == null || ((ICommunicationObject)player2Callback).State != CommunicationState.Opened)
            {
                game.Callbacks.Remove(player2Callback);
                this.HandleGameError(game);
                return;
            }

            player2Callback.StartGame(game.Id, game.Player1Name, game.Player2VisibleWindows, !player1Start);
        }

        /// <summary>
        /// Handles a game error and sends a callback to all still connected clients.
        /// </summary>
        /// <param name="game">The game.</param>
        private void HandleGameError(Game game)
        {
            game.Status = GameStatus.Canceled;
            foreach (var callback in game.Callbacks)
            {
                if (((ICommunicationObject)callback).State != CommunicationState.Opened)
                {
                    callback.GameError(game.Id);
                }
            }
        }

        /// <summary>
        /// Gets all games.
        /// </summary>
        /// <returns>All games, converted to a list</returns>
        private IEnumerable<Game> GetAllGames()
        {
            lock (LockObject)
            {
                return this.games.Values.ToList();
            }
        }
    }
}
