namespace Billapong.GameConsole.Models.Events
{
    using System;

    public class BallPlacedOnGameFieldEventArgs : EventArgs
    {
        public long WindowId { get; private set; }

        public int PointX { get; private set; }

        public int PointY { get; private set; }

        public BallPlacedOnGameFieldEventArgs(long windowId, int pointX, int pointY)
        {
            this.WindowId = windowId;
            this.PointX = pointX;
            this.PointY = pointY;
        }

    }
}
