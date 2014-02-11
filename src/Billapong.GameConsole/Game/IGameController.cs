namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    /// <summary>
    /// Defines the game behavior
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Occurs when ball is placed on the game field
        /// </summary>
        event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField;

        /// <summary>
        /// Occurs when the round has started
        /// </summary>
        event EventHandler<RoundStartedEventArgs> RoundStarted;

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        void PlaceBallOnGameField(long windowId, Point position);

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        void StartRound(Vector direction);
    }
}
