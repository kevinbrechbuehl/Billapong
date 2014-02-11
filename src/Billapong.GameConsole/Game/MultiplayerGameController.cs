namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;
    using Service;

    public class MultiplayerGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
        public event EventHandler<RoundStartedEventArgs> RoundStarted = delegate { };

        public MultiplayerGameController()
        {
            GameConsoleContext.Current.GameConsoleCallback.StartPointSet += this.OnBallPlacedOnGameField;
        }

        public void PlaceBallOnGameField(long windowId, Point position)
        {
            GameConsoleContext.Current.GameConsoleServiceClient.SetStartPoint(GameManager.Current.CurrentGame.GameId, windowId, position.X, position.Y);
        }

        public void StartRound(Point position)
        {
            GameConsoleContext.Current.GameConsoleServiceClient.StartRound(GameManager.Current.CurrentGame.GameId, position.X, position.Y);
        }

        public void OnRoundStarted(object sender, RoundStartedEventArgs args)
        {
            this.RoundStarted(this, args);
        }

        public void OnBallPlacedOnGameField(object sender, BallPlacedOnGameFieldEventArgs args)
        {
            this.BallPlacedOnGameField(this, args);
        }
    }
}
