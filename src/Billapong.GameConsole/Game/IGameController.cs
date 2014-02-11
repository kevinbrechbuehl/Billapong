namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    public interface IGameController
    {
        event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField;     

        void PlaceBallOnGameField(long windowId, Point position);
    }
}
