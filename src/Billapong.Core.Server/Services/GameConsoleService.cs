namespace Billapong.Core.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Contract.Data.GamePlay;
    using Contract.Data.Map;
    using Contract.Exceptions;
    using Contract.Service;
    using Converter.GamePlay;
    using Converter.Map;
    using GamePlay;
    using Map;

    /// <summary>
    /// The game console service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameConsoleService : IGameConsoleService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>
        /// Playable maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return MapController.Current.GetMaps(true).Select(map => map.ToContract()).ToList();
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
            var map = MapController.Current.GetMapById(id, true);
            return map != null ? map.ToContract() : null;
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
            return GameController.Current.OpenGame(mapId, visibleWindows, username, this.GetCallback());
        }

        /// <summary>
        /// Gets the lobby games.
        /// </summary>
        /// <returns>
        /// All open games in the lobby
        /// </returns>
        public IEnumerable<LobbyGame> GetLobbyGames()
        {
            return GameController.Current.GetOpenGames().Select(game => game.ToContract()).ToList();
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="gameId">The game identifier / correlation id of the game.</param>
        /// <param name="username">The username.</param>
        /// <exception cref="GameNotOpenException">
        /// The game was not found on the server or the game is not open
        /// </exception>
        public void JoinGame(Guid gameId, string username)
        {
            GameController.Current.JoinGame(gameId, username, this.GetCallback());
        }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <returns>Callback channel for the current context</returns>
        private IGameConsoleCallback GetCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IGameConsoleCallback>();
        }
    }
}
