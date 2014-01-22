namespace Billapong.GameConsole.Models.Events
{
    using System;
    using System.Windows;

    /// <summary>
    /// Event arguments of the GameFieldClicked event
    /// </summary>
    public class GameFieldClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameFieldClickedEventArgs"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public GameFieldClickedEventArgs(Point position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position { get; private set; }
    }
}
