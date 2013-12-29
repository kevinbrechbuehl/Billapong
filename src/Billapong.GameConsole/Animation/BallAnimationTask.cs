namespace Billapong.GameConsole.Animation
{
    using System;
    using System.Windows;

    /// <summary>
    /// Represents a single animation of the game ball
    /// </summary>
    public class BallAnimationTask
    {
        /// <summary>
        /// Gets or sets the new position.
        /// </summary>
        /// <value>
        /// The new position.
        /// </value>
        public Point NewPosition { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }
    }
}
