namespace Billapong.GameConsole.Models.Events
{
    using System;
    using System.Windows;

    /// <summary>
    /// The event arguments 
    /// </summary>
    public class RoundStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundStartedEventArgs"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public RoundStartedEventArgs(Vector direction)
        {
            this.Direction = direction;
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Vector Direction { get; private set; }
    }
}
