namespace Billapong.Contract.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.Map;

    /// <summary>
    /// Callback service for the game console.
    /// </summary>
    public interface IGameConsoleCallback
    {
        /// <summary>
        /// Starts the game with a specific id.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="map">The map.</param>
        /// <param name="opponentName">Name of the opponent.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="startGame">if set to <c>true</c> the player who receives this callback should start the game.</param>
        [OperationContract(Name = "StartGame")]
        void StartGame(Guid gameId, Map map, string opponentName, IEnumerable<long> visibleWindows, bool startGame);

        /// <summary>
        /// An error in the game ocurrs and the game has to be canceled by the client.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        [OperationContract(Name = "GameError")]
        void GameError(Guid gameId);
    }
}
