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
        /// Initializes a new instance of the <see cref="GameFieldClickedEventArgs" /> class.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        public GameFieldClickedEventArgs(Point mousePosition)
        {
            this.MousePosition = mousePosition;
        }

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <value>
        /// The mouse position.
        /// </value>
        public Point MousePosition { get; private set; }
    }
}
