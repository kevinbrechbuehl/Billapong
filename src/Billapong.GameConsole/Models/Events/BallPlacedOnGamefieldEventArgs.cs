namespace Billapong.GameConsole.Models.Events
{
    using System;
    using System.Windows;

    public class BallPlacedOnGameFieldEventArgs : EventArgs
    {
        public long WindowId { get; private set; }

        public Point Position { get; private set; }

        public BallPlacedOnGameFieldEventArgs(long windowId, Point position)
        {
            this.WindowId = windowId;
            this.Position = position;
        }

    }
}
