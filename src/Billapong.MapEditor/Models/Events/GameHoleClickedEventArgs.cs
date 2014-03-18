namespace Billapong.MapEditor.Models.Events
{
    using System;

    /// <summary>
    /// Event arguments for clicking on a game hole
    /// </summary>
    public class GameHoleClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameHoleClickedEventArgs"/> class.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x.</param>
        /// <param name="windowY">The window y.</param>
        /// <param name="holeId">The hole identifier.</param>
        /// <param name="holeX">The hole x.</param>
        /// <param name="holeY">The hole y.</param>
        public GameHoleClickedEventArgs(long windowId, int windowX, int windowY, long holeId, int holeX = 0, int holeY = 0)
        {
            this.HoleId = holeId;
            this.HoleX = holeX;
            this.HoleY = holeY;
            this.WindowId = windowId;
            this.WindowX = windowX;
            this.WindowY = windowY;
        }

        /// <summary>
        /// Gets the hole identifier.
        /// </summary>
        /// <value>
        /// The hole identifier.
        /// </value>
        public long HoleId { get; private set; }

        /// <summary>
        /// Gets the hole x.
        /// </summary>
        /// <value>
        /// The hole x.
        /// </value>
        public int HoleX { get; private set; }

        /// <summary>
        /// Gets the hole y.
        /// </summary>
        /// <value>
        /// The hole y.
        /// </value>
        public int HoleY { get; private set; }

        /// <summary>
        /// Gets the window identifier.
        /// </summary>
        /// <value>
        /// The window identifier.
        /// </value>
        public long WindowId { get; private set; }

        /// <summary>
        /// Gets the window x.
        /// </summary>
        /// <value>
        /// The window x.
        /// </value>
        public int WindowX { get; private set; }

        /// <summary>
        /// Gets the window y.
        /// </summary>
        /// <value>
        /// The window y.
        /// </value>
        public int WindowY { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "WindowId={0}, WindowX={1}, Window={2}, HoleId={3}, HoleX={4}, HoleY={5}",
                this.WindowId,
                this.WindowX,
                this.WindowY,
                this.HoleId,
                this.HoleX,
                this.HoleY);
        }
    }
}
