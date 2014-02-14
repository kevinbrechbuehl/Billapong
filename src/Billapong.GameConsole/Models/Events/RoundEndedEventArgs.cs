namespace Billapong.GameConsole.Models.Events
{
    using System;

    /// <summary>
    /// The arguments of the GameStarted event
    /// </summary>
    public class RoundEndedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundEndedEventArgs"/> class.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="gameEnded">if set to <c>true</c> [game ended].</param>
        public RoundEndedEventArgs(int score, bool gameEnded)
        {
            this.Score = score;
            this.GameEnded = gameEnded;
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the game ended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the game ended; otherwise, <c>false</c>.
        /// </value>
        public bool GameEnded { get; private set; }
    }
}
