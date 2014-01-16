namespace Billapong.GameConsole.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Windows;
    using Contract.Data.Map;
    using Contract.Service;
    using Converter.Map;
    using Models.Events;

    /// <summary>
    /// The callback client for the game console service
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class GameConsoleCallbackClient : IGameConsoleCallback
    {
        /// <summary>
        /// Occurs when the game starts.
        /// </summary>
        public event EventHandler<GameStartedEventArgs> GameStarted = delegate { };

        public void StartGame(Guid gameId, Map map, string opponentName, IEnumerable<long> visibleWindows, bool startGame)
        {
            if (this.GameStarted != null)
            {
                var args = new GameStartedEventArgs(gameId, map.ToEntity(visibleWindows), opponentName, startGame);
                this.GameStarted(this, args);
            }
        }

        public void GameError(Guid gameId)
        {
            MessageBox.Show("Upps something went wrong, need to cancel the game...");
        }

        public void CancelGame(Guid gameId)
        {
            MessageBox.Show("Someone/-thing has canceled the game...");
        }
    }
}
