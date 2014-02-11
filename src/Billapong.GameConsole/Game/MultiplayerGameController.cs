namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Models.Events;
    using Service;

    /// <summary>
    /// Controls the game behavior for a multiplayer game
    /// </summary>
    public class MultiplayerGameController : IGameController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerGameController"/> class.
        /// </summary>
        public MultiplayerGameController()
        {
            GameConsoleContext.Current.GameConsoleCallback.StartPointSet += this.OnBallPlacedOnGameField;
            GameConsoleContext.Current.GameConsoleCallback.RoundStarted += this.OnRoundStarted;
        }

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
            GameConsoleContext.Current.GameConsoleServiceClient.SetStartPoint(GameManager.Current.CurrentGame.GameId, windowId, position.X, position.Y);
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void StartRound(Vector direction)
        {
            GameConsoleContext.Current.GameConsoleServiceClient.StartRound(GameManager.Current.CurrentGame.GameId, direction.X, direction.Y);
        }

        /// <summary>
        /// Called when the server sends the round start callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoundStartedEventArgs"/> instance containing the event data.</param>
        public void OnRoundStarted(object sender, RoundStartedEventArgs args)
        {
            this.RoundStarted(this, args);
        }

        /// <summary>
        /// Called when the server sends the callback to set the start point.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="BallPlacedOnGameFieldEventArgs"/> instance containing the event data.</param>
        public void OnBallPlacedOnGameField(object sender, BallPlacedOnGameFieldEventArgs args)
        {
            this.BallPlacedOnGameField(this, args);
        }
    }
}
