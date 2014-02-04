namespace Billapong.GameConsole.Game
{
    using System;
    using Models.Events;

    public interface IGameController
    {
        event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField;     

        void PlaceBallOnGameField(long windowId, int pointX, int pointY);
    }
}
