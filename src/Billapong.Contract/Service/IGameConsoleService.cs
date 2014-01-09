﻿namespace Billapong.Contract.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.Map;

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
        /// Opens a new game and go into the lobby.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>Id of the game, used as correlation id</returns>
        [OperationContract(Name = "OpenGame")]
        Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username);

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="gameId">The game identifier / correlation id of the game.</param>
        /// <param name="username">The username.</param>
        [OperationContract(Name = "JoinGame", IsOneWay = true)]
        void JoinGame(Guid gameId, string username);
    }
}
