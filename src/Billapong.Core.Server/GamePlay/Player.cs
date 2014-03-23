namespace Billapong.Core.Server.GamePlay
{
    using System.Collections.Generic;
    using Contract.Service;

    /// <summary>
    /// Model for a game player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.VisibleWindows = new List<long>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        /// Gets the visible windows.
        /// </summary>
        /// <value>
        /// The visible windows.
        /// </value>
        public List<long> VisibleWindows { get; private set; }

        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        public IGameConsoleCallback Callback { get; set; }
    }
}
