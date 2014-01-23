﻿namespace Billapong.Contract.Service
{
    using System;
    using Data.Map;
    using System.Collections.Generic;
    using System.ServiceModel;

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
        [OperationContract(Name = "GameError")]
        void GameError();

        /// <summary>
        /// Cancels the game.
        /// </summary>
        [OperationContract(Name = "CancelGame")]
        void CancelGame();

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="windowsId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        [OperationContract(Name = "SetStartPoint")]
        void SetStartPoint(long windowsId, int pointX, int pointY);

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="pointX">The point x where user clicked to start the ball.</param>
        /// <param name="pointY">The point y where user clicked to start the ball.</param>
        [OperationContract(Name = "StartRound")]
        void StartRound(int pointX, int pointY);

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="score">The score.</param>
        [OperationContract(Name = "EndRound")]
        void EndRound(int score);
    }
}
