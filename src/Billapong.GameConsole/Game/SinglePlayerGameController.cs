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
        public event EventHandler<RoundStartedEventArgs> RoundStarted = delegate { };

        /// <summary>
        /// Occurs when the round has ended
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> RoundEnded = delegate { };

        /// <summary>
        /// Occurs when the game got cancelled by a player.
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> GameCancelled = delegate { };

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

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> [first player].</param>
        /// <param name="score">The score.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void EndRound(bool firstPlayer, int score)
        {
            throw new NotImplementedException();
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