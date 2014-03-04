namespace Billapong.GameConsole.Models.Events
{
    using System;
    using System.Windows;

    /// <summary>
    /// Event arguments for the BallPlacedOnGameField event
    /// </summary>
    public class BallPlacedOnGameFieldEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BallPlacedOnGameFieldEventArgs"/> class.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        public BallPlacedOnGameFieldEventArgs(long windowId, Point position)
        {
            this.WindowId = windowId;
            this.Position = position;
        }

        /// <summary>
        /// Gets the window identifier.
        /// </summary>
        /// <value>
        /// The window identifier.
        /// </value>
        public long WindowId { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position { get; private set; }
    }
}
