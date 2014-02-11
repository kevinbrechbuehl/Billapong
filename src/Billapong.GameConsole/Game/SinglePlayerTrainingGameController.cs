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
    }
}
