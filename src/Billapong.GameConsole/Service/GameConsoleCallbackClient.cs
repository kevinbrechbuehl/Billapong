namespace Billapong.GameConsole.Service
{
    using System;
    using System.ServiceModel;
    using System.Windows;
    using Contract.Service;

    [CallbackBehavior(UseSynchronizationContext = true)]
    public class GameConsoleCallbackClient : IGameConsoleCallback
    {
        public void StartGame(Guid gameId, string firstPlayer, string secondPlayer)
        {
            MessageBox.Show("Yay!");
        }
    }
}
