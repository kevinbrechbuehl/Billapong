namespace Billapong.GameConsole.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contract.Data.GamePlay;
    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client;

    /// <summary>
    /// Game console service client
    /// </summary>
    public class GameConsoleServiceClient : CallbackClientBase<IGameConsoleService, IGameConsoleCallback>, IGameConsoleService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsoleServiceClient"/> class.
        /// </summary>
        public GameConsoleServiceClient() : base(new GameConsoleCallback())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConsoleServiceClient"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public GameConsoleServiceClient(IGameConsoleCallback callback) : base(callback)
        {
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>
        /// Playable maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return this.Execute(() => this.Proxy.GetMaps());
        }

        /// <summary>
        /// Gets the maps asynchronous.
        /// </summary>
        /// <returns>The maps.</returns>
        public async Task<IEnumerable<Map>> GetMapsAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMaps());
        }

        /// <summary>
        /// Opens a new game on the server
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>
        /// Id of the game, used as correlation id
        /// </returns>
        public Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username)
        {
            return this.Execute(() => this.Proxy.OpenGame(mapId, visibleWindows, username));
        }

        /// <summary>
        /// Opens the game asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>The guid of the game</returns>
        public async Task<Guid> OpenGameAsync(long mapId, IEnumerable<long> visibleWindows, string username)
        {
            return await this.ExecuteAsync(() => this.Proxy.OpenGame(mapId, visibleWindows, username));
        }

        /// <summary>
        /// Gets the lobby games.
        /// </summary>
        /// <returns>The open games.</returns>
        public IEnumerable<LobbyGame> GetLobbyGames()
        {
            return this.Execute(() => this.Proxy.GetLobbyGames());
        }

        /// <summary>
        /// Gets the lobby games asynchronous.
        /// </summary>
        /// <returns>The open games.</returns>
        public async Task<IEnumerable<LobbyGame>> GetLobbyGamesAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetLobbyGames());
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="gameId">The game identifier / correlation id of the game.</param>
        /// <param name="username">The username.</param>
        public void JoinGame(Guid gameId, string username)
        {
            this.Execute(() => this.Proxy.JoinGame(gameId, username));
        }

        /// <summary>
        /// Joins the game asynchronous.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns>The task</returns>
        public async Task JoinGameAsync(Guid gameId, string username)
        {
            await this.ExecuteAsync(() => this.Proxy.JoinGame(gameId, username));
        }

        /// <summary>
        /// Cancels a game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isFirstPlayer">if set to <c>true</c> the player who cancels the game is the first player.</param>
        /// <param name="sendCallback">if set to <c>true</c> the player who cancels the game does not want to get a callback (mainly because he closes the window).</param>
        public void CancelGame(Guid gameId, bool isFirstPlayer, bool sendCallback)
        {
           this.Execute(() => this.Proxy.CancelGame(gameId, isFirstPlayer, sendCallback));
        }

        /// <summary>
        /// Cancels a game asynchronous.
        /// </summary>
        /// <param name="gameId">The game identifier.</param> 
        /// <param name="isFirstPlayer">if set to <c>true</c> the player who cancels the game is the first player.</param>
        /// <param name="sendCallback">if set to <c>true</c> the player who cancels the game does not want to get a callback (mainly because he closes the window).</param>
        /// <returns>The task</returns>
        public async Task CancelGameAsync(Guid gameId, bool isFirstPlayer, bool sendCallback)
        {
            await this.ExecuteAsync(() => this.Proxy.CancelGame(gameId, isFirstPlayer, sendCallback));
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        public void SetStartPoint(Guid gameId, long windowId, double pointX, double pointY)
        {
            this.Execute(() => this.Proxy.SetStartPoint(gameId, windowId, pointX, pointY));
        }

        /// <summary>
        /// Sets the start point asynchronous.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        /// <returns>The task</returns>
        public async Task SetStartPointAsync(Guid gameId, long windowId, double pointX, double pointY)
        {
            await this.ExecuteAsync(() => this.Proxy.SetStartPoint(gameId, windowId, pointX, pointY));
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="directionX">The direction x.</param>
        /// <param name="directionY">The direction y.</param>
        public void StartRound(Guid gameId, double directionX, double directionY)
        {
           this.Execute(() => this.Proxy.StartRound(gameId, directionX, directionY));
        }

        /// <summary>
        /// Starts the round asynchronous.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="directionX">The direction x.</param>
        /// <param name="directionY">The direction y.</param>
        /// <returns>The task</returns>
        public async Task StartRoundAsync(Guid gameId, double directionX, double directionY)
        {
            await this.ExecuteAsync(() => this.Proxy.StartRound(gameId, directionX, directionY));
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isPlayer1">if set to <c>true</c> user it player 1.</param>
        /// <param name="score">The score of the current round.</param>
        public void EndRound(Guid gameId, bool isPlayer1, int score)
        {
            this.Execute(() => this.Proxy.EndRound(gameId, isPlayer1, score));
        }

        /// <summary>
        /// Ends the round asynchronous.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isPlayer1">if set to <c>true</c> user it player 1.</param>
        /// <param name="score">The score of the current round.</param>
        /// <returns>The task</returns>
        public async Task EndRoundAsync(Guid gameId, bool isPlayer1, int score)
        {
            await this.ExecuteAsync(() => this.Proxy.EndRound(gameId, isPlayer1, score));
        }

        /// <summary>
        /// Adds the high score.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        public void AddHighScore(long mapId, string playerName, int score)
        {
           this.Execute(() => this.Proxy.AddHighScore(mapId, playerName, score));
        }

        /// <summary>
        /// Determines whether a specific game is still running and callbacks are valid.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        /// Boolean value if game is still running and valid
        /// </returns>
        public bool IsGameRunning(Guid gameId)
        {
            return this.Execute(() => this.Proxy.IsGameRunning(gameId));
        }

        /// <summary>
        /// Adds the high score asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        /// <returns>The task</returns>
        public async Task AddHighScoreAsync(long mapId, string playerName, int score)
        {
            await this.ExecuteAsync(() => this.Proxy.AddHighScore(mapId, playerName, score));
        }
    }
}