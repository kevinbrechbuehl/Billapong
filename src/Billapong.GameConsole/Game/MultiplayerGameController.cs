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
        /// Occurs when an exception is catched that needs to end the game
        /// </summary>
        public event EventHandler ErrorOccurred = delegate { };

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="position">The position.</param>
        public async void PlaceBallOnGameField(long windowId, Point position)
        {
            try
            {
                await GameConsoleContext.Current.GameConsoleServiceClient.SetStartPointAsync(
                    GameManager.Current.CurrentGame.GameId,
                    windowId,
                    position.X,
                    position.Y);
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
            }
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public async void StartRound(Vector direction)
        {
            try
            {
                await
                    GameConsoleContext.Current.GameConsoleServiceClient.StartRoundAsync(
                        GameManager.Current.CurrentGame.GameId,
                        direction.X,
                        direction.Y);
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
            }
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> the current round was played by the first player.</param>
        /// <param name="score">The score.</param>
        public async void EndRound(bool firstPlayer, int score)
        {
            this.currentRoundEnded = true;

            // We only want to send the end round command to the server if this was the current players turn
            if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
            {
                try
                {
                    await GameConsoleContext.Current.GameConsoleServiceClient.EndRoundAsync(
                        GameManager.Current.CurrentGame.GameId,
                        firstPlayer,
                        score);
                }
                catch (Exception ex)
                {
                    Tracer.Error(ex.Message, ex);
                    this.ErrorOccurred(this, null);
                }
            }
            else if (this.currentRoundEndedEventRecieved && this.currentRoundEndedArguments != null)
            {
                // Fire the RoundEnded event if the server already sent the appropriate event
                this.OnRoundEnded(this.currentRoundEndedArguments);
            }
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public async void CancelGame()
        {
            try
            {
                await GameConsoleContext.Current.GameConsoleServiceClient.CancelGameAsync(GameManager.Current.CurrentGame.GameId);
            }
            catch (Exception ex)
            {
                // We log the error and close the game, but we do not resend the CancelGame command because it failed already
                this.HandleError(ex, false);
            }
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
            if (this.currentRoundEnded)
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
                const string WonLogMessage = "The game ended. {0} won the game with a score of {1} points against {2} with a score of {3}";
                if (GameManager.Current.CurrentGame.LocalPlayer.Score >
                    GameManager.Current.CurrentGame.Opponent.Score)
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Won;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Lost;

                    if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(
                                WonLogMessage,
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score,
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score),
                            Tracer.Info);
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
                            string.Format(
                                WonLogMessage, 
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score,
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score), 
                                Tracer.Info);
                    }
                }
                else
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Draw;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Draw;

                    if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(
                                "The game ended. {0} and {1} played a draw with a score of {2}",
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score), 
                                Tracer.Info);
                    }
                }

                Tracer.ProcessQueuedMessages();

                this.GameEndCleanup();
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

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="sendCancelCommandToServer">if set to <c>true</c> the CancelGame command is sent to the server.</param>
        private void HandleError(Exception ex, bool sendCancelCommandToServer = true)
        {
            Tracer.Error(ex.Message, ex);
            if (sendCancelCommandToServer)
            {
                this.CancelGame();
            }

            this.ErrorOccurred(this, null);
        }

        /// <summary>
        /// Cleans up the current instance after a game is finished
        /// </summary>
        private void GameEndCleanup()
        {
            GameConsoleContext.Current.GameConsoleCallback.StartPointSet -= this.OnBallPlacedOnGameField;
            GameConsoleContext.Current.GameConsoleCallback.RoundStarted -= this.OnRoundStarted;
            GameConsoleContext.Current.GameConsoleCallback.RoundEnded -= this.OnRoundEnded;
            GameConsoleContext.Current.GameConsoleCallback.GameCancelled -= this.OnGameCanceled;
        }
    }
}
