namespace Billapong.GameConsole.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Windows;
    using Contract.Service;

    /// <summary>
    /// The callback client for the game console service
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class GameConsoleCallbackClient : IGameConsoleCallback
    {
        public void StartGame(Guid gameId, string opponentName, IEnumerable<long> visibleWindows, bool startGame)
        {
            MessageBox.Show(string.Format("Start Game: {0}{1}Opponent Name: {2}{3}Visible Windows:{4}{5}I start the game: {6}",
                gameId,
                Environment.NewLine,
                opponentName,
                Environment.NewLine,
                string.Join(", ", visibleWindows),
                Environment.NewLine,
                startGame));
        }

        public void GameError(Guid gameId)
        {
            MessageBox.Show("Upps something went wrong, need to cancel the game...");
        }
    }
}
