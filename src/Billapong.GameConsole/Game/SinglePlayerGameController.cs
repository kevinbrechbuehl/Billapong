namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    public class SinglePlayerGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
       
        public void PlaceBallOnGameField(long windowId, int pointX, int pointY)
        {
            throw new NotImplementedException();
        }
    }
}
