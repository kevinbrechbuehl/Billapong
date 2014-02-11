namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    public class SinglePlayerGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
        public event EventHandler<RoundStartedEventArgs> RoundStarted;

        public void PlaceBallOnGameField(long windowId, Point position)
        {
            throw new NotImplementedException();
        }

        public void StartRound(Point position)
        {
            throw new NotImplementedException();
        }
    }
}