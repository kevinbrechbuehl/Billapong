namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    public class SinglePlayerTrainingGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
        public event EventHandler<RoundStartedEventArgs> RoundStarted = delegate { };

        public void PlaceBallOnGameField(long windowId, Point position)
        {
            var eventArgs = new BallPlacedOnGameFieldEventArgs(windowId, position);
            this.BallPlacedOnGameField(this, eventArgs);
        }

        public void StartRound(Point position)
        {
            var eventArgs = new RoundStartedEventArgs();
            this.RoundStarted(this, eventArgs);
        }
    }
}
