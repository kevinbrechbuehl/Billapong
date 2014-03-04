namespace Billapong.GameConsole.Game
{
    using System;
    using System.Windows;
    using Core.Client.Tracing;
    using Models;
    using Models.Events;
    using Service;

    /// <summary>
    /// Controls the game behavior for a multiplayer game
    /// </summary>
    public class MultiplayerGameController : IGameController
    {
        /// <summary>
        /// Number of bounces
        /// </summary>
        private int bounceCount;

        /// <summary>
        /// Indicates, whether the current round has ended for this player
        /// </summary>
        private bool currentRoundEnded = false;

        /// <summary>
        /// Indicates, whether the game server sent the round ended event for the current round 
        /// </summary>
        private bool currentRoundEndedEventRecieved = false;

        /// <summary>
        /// The current round ended arguments
        /// </summary>
        private RoundEndedEventArgs currentRoundEndedArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplayerGameController"/> class.
        /// </summary>
        public MultiplayerGameController()
        {
            GameConsoleContext.Current.GameConsoleCallback.StartPointSet += this.OnBallPlacedOnGameField;
            GameConsoleContext.Current.GameConsoleCallback.RoundStarted += this.OnRoundStarted;
            GameConsoleContext.Current.GameConsoleCallback.RoundEnded += this.OnRoundEnded;
            GameConsoleContext.Current.GameConsoleCallback.GameCancelled += this.OnGameCanceled;
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
        /// Occurs when the round has ended
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> RoundEnded = delegate { };

        /// <summary>
        /// Occurs when the game got cancelled by a player.
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> GameCanceled = delegate { };

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
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> the current round was played by the first player.</param>
        /// <param name="score">The score.</param>
        public void EndRound(bool firstPlayer, int score)
        {
            currentRoundEnded = true;

            // We only want to send the end round command to the server if this was the current players turn
            if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
            {
                GameConsoleContext.Current.GameConsoleServiceClient.EndRound(GameManager.Current.CurrentGame.GameId, firstPlayer, score);
            }
            // Fire the RoundEnded event if the server already sent the appropriate event
            else if (this.currentRoundEndedEventRecieved && this.currentRoundEndedArguments != null)
            {
                this.OnRoundEnded(this.currentRoundEndedArguments);
            }
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            GameConsoleContext.Current.GameConsoleServiceClient.CancelGame(GameManager.Current.CurrentGame.GameId);
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
        /// Called when the server sends the round ended callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoundEndedEventArgs"/> instance containing the event data.</param>
        public void OnRoundEnded(object sender, RoundEndedEventArgs args)
        {
            if (currentRoundEnded)
            {
                this.OnRoundEnded(args);
            }
            else
            {
                this.currentRoundEndedEventRecieved = true;
                this.currentRoundEndedArguments = args;
                GameManager.Current.LogMessage("The round was out of sync and gets now synchronized again", Tracer.Debug);
            }
        }

        /// <summary>
        /// Gets called when the server sent the RoundEnded event and the local player is ready for the next round
        /// </summary>
        /// <param name="args">The <see cref="RoundEndedEventArgs"/> instance containing the event data.</param>
        public void OnRoundEnded(RoundEndedEventArgs args)
        {
            GameManager.Current.CurrentGame.CurrentPlayer.Score = args.Score;
            GameManager.Current.CurrentGame.CurrentPlayer.CurrentPlayerState = Player.PlayerState.OpponentsTurn;

            this.bounceCount++;

            if (!args.GameEnded)
            {
                if (this.bounceCount % 2 == 0 && this.bounceCount > 1)
                {
                    GameManager.Current.CurrentGame.CurrentRound = (this.bounceCount / 2) + 1;
                }

                var game = GameManager.Current.CurrentGame;
                GameManager.Current.CurrentGame.CurrentPlayer = game.CurrentPlayer == game.LocalPlayer
                    ? game.Opponent
                    : game.LocalPlayer;
            }
            else
            {
                const string wonLogMessage = "The game ended. {0} won the game with a score of {1} points against {2} with a score of {3}";
                if (GameManager.Current.CurrentGame.LocalPlayer.Score >
                    GameManager.Current.CurrentGame.Opponent.Score)
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Won;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Lost;

                    if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(wonLogMessage, GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score,
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score), Tracer.Info);
                    }
                }
                else if (GameManager.Current.CurrentGame.LocalPlayer.Score <
                            GameManager.Current.CurrentGame.Opponent.Score)
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Lost;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Won;

                    if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(wonLogMessage, GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score,
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score), Tracer.Info);
                    }
                }
                else
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Draw;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Draw;

                    if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                    {
                        GameManager.Current.LogMessage(
                            string.Format("The game ended. {0} and {1} played a draw with a score of {2}",
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score), Tracer.Info);
                    }
                }

                Tracer.ProcessQueuedMessages();
            }

            this.RoundEnded(this, args);

            // Reset the data for the current round
            this.currentRoundEnded = false;
            this.currentRoundEndedEventRecieved = false;
            this.currentRoundEndedArguments = null;
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

        /// <summary>
        /// Called when the server sends the callback to cancel the game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void OnGameCanceled(object sender, EventArgs args)
        {
            this.GameCanceled(this, null);
        }
    }
}
