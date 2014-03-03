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
        /// Opens a new game and go into the lobby.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>Id of the game, used as correlation id</returns>
        [OperationContract(Name = "OpenGame")]
        [FaultContract(typeof(MapNotFoundException))]
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
        [OperationContract(Name = "JoinGame")]
        [FaultContract(typeof(GameNotOpenException))]
        [FaultContract(typeof(GameNotFoundException))]
        void JoinGame(Guid gameId, string username);

        /// <summary>
        /// Cancels a game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        [OperationContract(Name = "CancelGame")]
        void CancelGame(Guid gameId);

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        [OperationContract(Name = "SetStartPoint")]
        void SetStartPoint(Guid gameId, long windowId, double pointX, double pointY);

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="pointX">The point x where the user clicked to start the ball.</param>
        /// <param name="directionY">The point y where the user clicked to start the ball.</param>
        [OperationContract(Name = "StartRound")]
        void StartRound(Guid gameId, double pointX, double directionY);

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isPlayer1">if set to <c>true</c> user it player 1.</param>
        /// <param name="score">The score of the current round.</param>
        [OperationContract(Name = "EndRound")]
        void EndRound(Guid gameId, bool isPlayer1, int score);

        /// <summary>
        /// Adds the high score.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        [OperationContract(Name = "AddHighScore")]
        void AddHighScore(long mapId, string playerName, int score);
    }
}
