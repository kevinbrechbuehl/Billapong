namespace Billapong.GameConsole.Service
{
    using System;
    using Contract.Service;

    public class GameConsoleCallbackClient : IGameConsoleCallback
    {
        public void StartGame(Guid gameId, string firstPlayer, string secondPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
