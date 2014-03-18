namespace Billapong.MapEditor.Models.Parameters
{
    using System.Windows;

    /// <summary>
    /// Event arguments for clicking on a game window.
    /// </summary>
    public class GameWindowClickedArgs
    {
        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public Point Point { get; set; }

        /// <summary>
        /// Gets or sets the game window.
        /// </summary>
        /// <value>
        /// The game window.
        /// </value>
        public GameWindow GameWindow { get; set; }
    }
}
