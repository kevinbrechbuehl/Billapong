namespace Billapong.GameConsole.Game
{
    using System;
    using Models.Events;

    public class SinglePlayerTrainingGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
        public void PlaceBallOnGameField(long windowId, int pointX, int pointY)
        {
            var eventArgs = new BallPlacedOnGameFieldEventArgs(windowId, pointX, pointY);
            this.BallPlacedOnGameField(this, eventArgs);
        }
    }
}
