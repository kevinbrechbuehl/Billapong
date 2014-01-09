namespace Billapong.GameConsole.Service
{
    using System;
    using System.ServiceModel;
    using System.Windows;
    using Contract.Service;

    /// <summary>
    /// The callback client for the game console service
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class GameConsoleCallbackClient : IGameConsoleCallback
    {
        /// <summary>
        /// Starts the game with a specific id.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="firstPlayer">The first player, who starts the game.</param>
        /// <param name="secondPlayer">The second player.</param>
        public void StartGame(Guid gameId, string firstPlayer, string secondPlayer)
        {
            MessageBox.Show("Starting game!");
        }
    }
}
