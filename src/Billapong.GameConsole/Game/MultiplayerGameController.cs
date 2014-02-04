namespace Billapong.GameConsole.Game
{
    using System;
    using Models.Events;
    using Service;

    public class MultiplayerGameController : IGameController
    {
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };

        private readonly GameConsoleServiceClient consoleServiceClient;

        public MultiplayerGameController()
        {
            var consoleCallback = new GameConsoleCallback();
            consoleCallback.StartPointSet += OnBallPlacedOnGameField;
            this.consoleServiceClient = new GameConsoleServiceClient(consoleCallback);
        }

        public void PlaceBallOnGameField(long windowId, int pointX, int pointY)
        {
            this.consoleServiceClient.SetStartPoint(GameManager.Instance.CurrentGame.GameId, windowId, pointX, pointY);
        }

        public void OnBallPlacedOnGameField(object sender, BallPlacedOnGameFieldEventArgs args)
        {
            this.BallPlacedOnGameField(this, args);
        }
    }
}
