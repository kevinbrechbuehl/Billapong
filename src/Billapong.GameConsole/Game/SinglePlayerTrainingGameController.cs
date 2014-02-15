namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    /// <summary>
    /// Controls the game behavior for a single player training game
    /// </summary>
    public class SinglePlayerTrainingGameController : IGameController
    {
        /// <summary>
        /// Occurs when ball is placed on the game field
        /// </summary>
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };

        /// <summary>
        /// Occurs when the round has started
        /// </summary>
        public event EventHandler<RoundStartedEventArgs> RoundStarted = delegate { };

        /// <summary>
        /// Occurs when the round has ended
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> RoundEnded = delegate { };

        /// <summary>
        /// Occurs when the game got cancelled by a player.
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> GameCancelled;

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        public void PlaceBallOnGameField(long windowId, Point position)
        {
            var eventArgs = new BallPlacedOnGameFieldEventArgs(windowId, position);
            this.BallPlacedOnGameField(this, eventArgs);
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void StartRound(Vector direction)
        {
            var eventArgs = new RoundStartedEventArgs(direction);
            this.RoundStarted(this, eventArgs);
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> the current player started the game. This is necessary for the server to identify the player.</param>
        /// <param name="score">The score.</param>
        public void EndRound(bool firstPlayer, int score)
        {
            var eventArgs = new RoundEndedEventArgs(score, false);
            this.RoundEnded(this, eventArgs);
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            this.GameCancelled(this, null);
        }
    }
}
