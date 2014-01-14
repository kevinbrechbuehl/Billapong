namespace Billapong.Contract.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.GamePlay;
    using Data.Map;
    using Exceptions;

    /// <summary>
    /// The game console service contract
    /// </summary>
    [ServiceContract(Name = "GameConsole", CallbackContract = typeof(IGameConsoleCallback), Namespace = Globals.ServiceContractNamespaceName)]
    public interface IGameConsoleService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>Playable maps in the database</returns>
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps();

        /// <summary>
        /// Gets the map by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The map, if valid map is found, null otheriwse</returns>
        [OperationContract(Name = "GetMapById")]
        Map GetMapById(long id);

        /// <summary>
        /// Opens a new game and go into the lobby.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>Id of the game, used as correlation id</returns>
        [OperationContract(Name = "OpenGame")]
        Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username);

        /// <summary>
        /// Gets the lobby games.
        /// </summary>
        /// <returns>All open games in the lobby</returns>
        [OperationContract(Name = "GetLobbyGames")]
        IEnumerable<LobbyGame> GetLobbyGames();

            /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="gameId">The game identifier / correlation id of the game.</param>
        /// <param name="username">The username.</param>
        /// <exception cref="GameNotFoundException">The game was not found on the server</exception>
        /// <exception cref="GameNotOpenException">The game is not open</exception>
        [OperationContract(Name = "JoinGame")]
        void JoinGame(Guid gameId, string username);
    }
}
