namespace Billapong.Contract.Service
{
    using System;
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
        /// <param name="firstPlayer">The first player, who starts the game.</param>
        /// <param name="secondPlayer">The second player.</param>
        [OperationContract(Name = "StartGame")]
        void StartGame(Guid gameId, string firstPlayer, string secondPlayer);
    }
}
