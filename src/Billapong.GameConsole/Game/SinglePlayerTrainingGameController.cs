namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    public class SinglePlayerTrainingGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };

        public void PlaceBallOnGameField(long windowId, Point position)
        {
            var eventArgs = new BallPlacedOnGameFieldEventArgs(windowId, position);
            this.BallPlacedOnGameField(this, eventArgs);
        }
    }
}
