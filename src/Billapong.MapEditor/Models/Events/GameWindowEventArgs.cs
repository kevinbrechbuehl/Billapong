namespace Billapong.MapEditor.Models.Events
{
    using System;

    /// <summary>
    /// Event arguments for a game window.
    /// </summary>
    public class GameWindowEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public GameWindowEventArgs(long id, int x = 0, int y = 0)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; private set; }

        /// <summary>
        /// Gets the x coords.
        /// </summary>
        /// <value>
        /// The x coords.
        /// </value>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y coords.
        /// </summary>
        /// <value>
        /// The y coords.
        /// </value>
        public int Y { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id={0}, X={1}, Y={2}", this.Id, this.X, this.Y);
        }
    }
}
