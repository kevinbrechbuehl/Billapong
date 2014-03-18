namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Billapong.Core.Client.Tracing;
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
        public event EventHandler<RoundEndedEventArgs> GameCanceled = delegate { };

        /// <summary>
        /// Occurs when an exception is catched that needs to end the game
        /// </summary>
        public event EventHandler ErrorOccurred = delegate { };

        /// <summary>
        /// Can be used to log something at the start of the game
        /// </summary>
        public void StartGame()
        {
            GameManager.Current.LogMessage(
                    string.Format("Starting a new singleplayer training"),
                    Tracer.Info);
        }

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        public void PlaceBallOnGameField(long windowId, Point position)
        {
            GameManager.Current.LogMessage(
                string.Format(
                    "Placed ball in window with id {0} on position {1}",
                    windowId,
                    position),
                Tracer.Debug);
            var eventArgs = new BallPlacedOnGameFieldEventArgs(windowId, position);
            this.BallPlacedOnGameField(this, eventArgs);
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void StartRound(Vector direction)
        {
            GameManager.Current.LogMessage(
                string.Format("Started round with ball direction {0}", direction),
                Tracer.Debug);
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
            GameManager.Current.LogMessage(
                string.Format("Finished round with a score of {0}", score),
                Tracer.Debug);
            GameManager.Current.CurrentGame.CurrentRound++;
            GameManager.Current.CurrentGame.CurrentPlayer.Score += score;
            var eventArgs = new RoundEndedEventArgs(score, false);
            this.RoundEnded(this, eventArgs);
        }

        /// <summary>
        /// Can be used to log something at the end of a game (Does not happen in this mode ;))
        /// </summary>
        public void EndGame()
        {
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            GameManager.Current.LogMessage(
                string.Format("Canceled the game"),
                Tracer.Debug);
            this.GameCanceled(this, null);
        }
    }
}
