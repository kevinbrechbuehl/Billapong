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
        /// Occurs when the round has ended
        /// </summary>
        event EventHandler<RoundEndedEventArgs> RoundEnded;

        /// <summary>
        /// Occurs when the game got canceled by a player.
        /// </summary>
        event EventHandler<RoundEndedEventArgs> GameCanceled;

        /// <summary>
        /// Occurs when an exception is catched that needs to end the game
        /// </summary>
        event EventHandler ErrorOccurred;

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

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> [first player].</param>
        /// <param name="score">The score.</param>
        void EndRound(bool firstPlayer, int score);

        /// <summary>
        /// Cancels the game.
        /// </summary>
        void CancelGame();
    }
}
