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
        /// Gets the map by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The map, if valid map is found, null otheriwse
        /// </returns>
        public Map GetMapById(long id)
        {
            return this.Execute(() => this.Proxy.GetMapById(id));
        }

        /// <summary>
        /// Opens a new game and go into the lobby.
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
        /// Cancels a game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        public void CancelGame(Guid gameId)
        {
            this.Execute(() => this.Proxy.CancelGame(gameId));
        }

        public void SetStartPoint(Guid gameId, long windowId, double pointX, double pointY)
        {
            this.Execute(() => this.Proxy.SetStartPoint(gameId, windowId, pointX, pointY));
        }

        public void StartRound(Guid gameId, double directionX, double directionY)
        {
            this.Execute(() => this.Proxy.StartRound(gameId, directionX, directionY));
        }

        public void EndRound(Guid gameId, bool isPlayer1, int score)
        {
            this.Execute(() => this.Proxy.EndRound(gameId, isPlayer1, score));
        }

        /// <summary>
        /// Gets the maps asynchronous.
        /// </summary>
        /// <returns>The maps.</returns>
        public async Task<IEnumerable<Map>> GetMapsAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMaps());
        }
    }
}
