namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Billapong.Core.Client.Tracing;
    using Billapong.GameConsole.Models;
    using Billapong.GameConsole.Service;
    using Models.Events;

    /// <summary>
    /// Controls the game behavior for a game against the computer
    /// </summary>
    public class SinglePlayerGameController : IGameController
    {
        /// <summary>
        /// Defines the time which the computer player waits between setting the ball and starting the round
        /// </summary>
        private readonly TimeSpan computerThinkingSimulationTime = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The windows which can be used for setting the ball for the computer
        /// </summary>
        private readonly List<Models.Window> computerWindows = null;

        /// <summary>
        /// Number of bounces
        /// </summary>
        private int bounceCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayerGameController"/> class.
        /// </summary>
        public SinglePlayerGameController()
        {
            // Get all usable windows for the computer
            this.computerWindows = GameManager.Current.CurrentGame.Map.Windows.Where(x => !x.IsOwnWindow).ToList();

            // If the player sees all windows, the computer has to use the same windows
            if (!this.computerWindows.Any())
            {
                this.computerWindows = GameManager.Current.CurrentGame.Map.Windows.ToList();
            }
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

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="firstPlayer">if set to <c>true</c> [first player].</param>
        /// <param name="score">The score.</param>
        public async void EndRound(bool firstPlayer, int score)
        {
            var gameEnded = false;
            GameManager.Current.CurrentGame.CurrentPlayer.Score += score;

            this.bounceCount++;
            if (this.bounceCount % 2 == 0 && this.bounceCount > 1)
            {
                var newRound = (this.bounceCount / 2) + 1;
                if (newRound > 10)
                {
                    gameEnded = true;
                }
                else
                {
                    GameManager.Current.CurrentGame.CurrentRound = newRound;
                }
            }

            if (gameEnded)
            {
                const string WonLogMessage = "The game ended. {0} won the singleplayer game with a score of {1} points against {2} with a score of {3}";
                if (GameManager.Current.CurrentGame.LocalPlayer.Score >
                    GameManager.Current.CurrentGame.Opponent.Score)
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Won;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Lost;
                    GameManager.Current.LogMessage(
                        string.Format(
                            WonLogMessage,
                            GameManager.Current.CurrentGame.LocalPlayer.Name,
                            GameManager.Current.CurrentGame.LocalPlayer.Score,
                            GameManager.Current.CurrentGame.Opponent.Name,
                            GameManager.Current.CurrentGame.Opponent.Score),
                        Tracer.Info);
                }
                else if (GameManager.Current.CurrentGame.LocalPlayer.Score <
                            GameManager.Current.CurrentGame.Opponent.Score)
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Lost;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Won;
                    GameManager.Current.LogMessage(
                        string.Format(
                            WonLogMessage,
                            GameManager.Current.CurrentGame.Opponent.Name,
                            GameManager.Current.CurrentGame.Opponent.Score,
                            GameManager.Current.CurrentGame.LocalPlayer.Name,
                            GameManager.Current.CurrentGame.LocalPlayer.Score),
                            Tracer.Info);
                }
                else
                {
                    GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState = Player.PlayerState.Draw;
                    GameManager.Current.CurrentGame.Opponent.CurrentPlayerState = Player.PlayerState.Draw;
                    GameManager.Current.LogMessage(
                        string.Format(
                            "The singleplayer game ended. {0} and {1} played a draw with a score of {2}",
                            GameManager.Current.CurrentGame.LocalPlayer.Name,
                            GameManager.Current.CurrentGame.Opponent.Name,
                            GameManager.Current.CurrentGame.LocalPlayer.Score),
                            Tracer.Info);
                }

                try
                {
                    // Send the highscore of the player to the server
                    await GameConsoleContext.Current.GameConsoleServiceClient.AddHighScoreAsync(
                        GameManager.Current.CurrentGame.Map.Id,
                        GameManager.Current.CurrentGame.LocalPlayer.Name,
                        GameManager.Current.CurrentGame.LocalPlayer.Score);
                }
                catch (Exception ex)
                {
                    this.LogError(ex.Message, ex);
                }

                await Tracer.ProcessQueuedMessages();
            }
            else 
            {
                GameManager.Current.CurrentGame.CurrentPlayer.CurrentPlayerState = Player.PlayerState.OpponentsTurn;
                if (GameManager.Current.CurrentGame.CurrentPlayer == GameManager.Current.CurrentGame.LocalPlayer)
                {
                    GameManager.Current.CurrentGame.CurrentPlayer = GameManager.Current.CurrentGame.Opponent;
                    await this.ComputerStartNewRound();
                }
                else
                {
                    GameManager.Current.CurrentGame.CurrentPlayer = GameManager.Current.CurrentGame.LocalPlayer;
                }
            }

            var eventArgs = new RoundEndedEventArgs(score, gameEnded);
            this.RoundEnded(this, eventArgs);
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            this.GameCanceled(this, null);
        }

        /// <summary>
        /// Calculates all relevant data and starts the round
        /// </summary>
        /// <returns>The task</returns>
        private async Task ComputerStartNewRound()
        {
            // Choose a random window out of the possible windows
            var randomWindow = GameHelpers.GetRandomWindow(this.computerWindows);
            if (randomWindow != null)
            {
                // Set the ball on a free grid part
                var ballPosition = GameHelpers.GetRandomBallPosition(randomWindow);
                if (ballPosition != null)
                {
                    // Place the ball on the selected window
                    this.PlaceBallOnGameField(randomWindow.Id, ballPosition.Value);

                    // Simulate "thinking" time :)
                    await Task.Delay(this.computerThinkingSimulationTime);

                    // Start the round in a direction which does not end up in a hole within the initial move
                    this.StartRound(GameHelpers.GetRandomBallDirection(randomWindow, ballPosition.Value));
                }
            }
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        private async void LogError(string message, Exception ex)
        {
            await Tracer.Error(message, ex);
        }
    }
}