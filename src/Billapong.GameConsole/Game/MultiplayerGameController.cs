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
        /// Initializes a new instance of the <see cref="MultiplayerGameController"/> class.
        /// </summary>
        public MultiplayerGameController()
        {
            GameConsoleContext.Current.GameConsoleCallback.StartPointSet += this.OnBallPlacedOnGameField;
            GameConsoleContext.Current.GameConsoleCallback.RoundStarted += this.OnRoundStarted;
            GameConsoleContext.Current.GameConsoleCallback.RoundEnded += this.OnRoundEnded;
            GameConsoleContext.Current.GameConsoleCallback.GameCancelled += this.OnGameCancelled;
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
        public event EventHandler<RoundEndedEventArgs> GameCancelled = delegate { };

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
            GameConsoleContext.Current.GameConsoleServiceClient.EndRound(GameManager.Current.CurrentGame.GameId, firstPlayer, score);
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
            GameManager.Current.CurrentGame.CurrentPlayer.Score = args.Score;
            GameManager.Current.CurrentGame.CurrentPlayer.CurrentRoundState = Player.RoundState.OpponentsTurn;

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
                if (GameManager.Current.CurrentGame.CurrentPlayer.IsLocalPlayer)
                {
                    const string wonLogMessage = "The game ended. {0} won the game with a score of {1} points against {2} with a score of {3}";
                    if (GameManager.Current.CurrentGame.LocalPlayer.Score >
                        GameManager.Current.CurrentGame.Opponent.Score)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(wonLogMessage, GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score,
                                GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score), Tracer.Info);
                    }
                    else if (GameManager.Current.CurrentGame.LocalPlayer.Score <
                             GameManager.Current.CurrentGame.Opponent.Score)
                    {
                        GameManager.Current.LogMessage(
                            string.Format(wonLogMessage, GameManager.Current.CurrentGame.Opponent.Name,
                                GameManager.Current.CurrentGame.Opponent.Score,
                                GameManager.Current.CurrentGame.LocalPlayer.Name,
                                GameManager.Current.CurrentGame.LocalPlayer.Score), Tracer.Info);
                    }
                    else
                    {
                        GameManager.Current.LogMessage(string.Format("The game ended. {0} and {1} played a draw with a score of {2}", GameManager.Current.CurrentGame.LocalPlayer.Name, GameManager.Current.CurrentGame.Opponent.Name, GameManager.Current.CurrentGame.LocalPlayer.Score), Tracer.Info);
                    }

                    Tracer.ProcessQueuedMessages();
                }
            }

            this.RoundEnded(this, args);
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
        public void OnGameCancelled(object sender, EventArgs args)
        {
            this.GameCancelled(this, null);
        }
    }
}
