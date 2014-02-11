namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;

    /// <summary>
    /// Controls the game behavior for a game against the computer
    /// </summary>
    public class SinglePlayerGameController : IGameController
    {
        /// <summary>
        /// Occurs when ball is placed on the game field
        /// </summary>
        public event EventHandler<BallPlacedOnGameFieldEventArgs> BallPlacedOnGameField = delegate { };
        
        /// <summary>
        /// Occurs when the round has started
        /// </summary>
        public event EventHandler<RoundStartedEventArgs> RoundStarted;

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void PlaceBallOnGameField(long windowId, Point position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void StartRound(Vector direction)
        {
            throw new NotImplementedException();
        }
    }
}