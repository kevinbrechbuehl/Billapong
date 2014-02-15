namespace Billapong.GameConsole.Service
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Windows;
    using Contract.Data.Map;
    using Contract.Service;
    using Converter.Map;
    using Core.Client.Helper;
    using Models.Events;

    /// <summary>
    /// The callback client for the game console service
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = true, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class GameConsoleCallback : IGameConsoleCallback
    {
        /// <summary>
        /// Occurs when the game starts.
        /// </summary>
        public event EventHandler<GameStartedEventArgs> GameStarted = delegate { };

        /// <summary>
        /// Occurs when the ball should be placed on the gamefield.
        /// </summary>
        public event EventHandler<BallPlacedOnGameFieldEventArgs> StartPointSet = delegate { };

        /// <summary>
        /// Occurs when the round starts.
        /// </summary>
        public event EventHandler<RoundStartedEventArgs> RoundStarted = delegate { };

        /// <summary>
        /// Occurs when the round ended.
        /// </summary>
        public event EventHandler<RoundEndedEventArgs> RoundEnded = delegate { };

        /// <summary>
        /// Occurs when the game got cancelled by a player.
        /// </summary>
        public event EventHandler GameCancelled = delegate { };

        /// <summary>
        /// Starts the game with a specific id.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="map">The map.</param>
        /// <param name="opponentName">Name of the opponent.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="startGame">if set to <c>true</c> the player who receives this callback should start the game.</param>
        public void StartGame(Guid gameId, Map map, string opponentName, IEnumerable<long> visibleWindows, bool startGame)
        {
            var args = new GameStartedEventArgs(gameId, map.ToEntity(visibleWindows), opponentName, startGame);
            ThreadContext.InvokeOnUiThread(() => this.OnGameStarted(args));
        }

        /// <summary>
        /// An error in the game ocurrs and the game has to be canceled by the client.
        /// </summary>
        public void GameError()
        {
            MessageBox.Show("Upps something went wrong, need to cancel the game...");
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            ThreadContext.InvokeOnUiThread(() => this.GameCancelled(this, null));
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        public void SetStartPoint(long windowId, double pointX, double pointY)
        {
            var args = new BallPlacedOnGameFieldEventArgs(windowId, new Point(pointX, pointY));
            ThreadContext.InvokeOnUiThread(() => this.OnStartPointSet(args));   
        }


        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="directionX">The direction x where user clicked to start the ball.</param>
        /// <param name="directionY">The direction y where user clicked to start the ball.</param>
        public void StartRound(double directionX, double directionY)
        {
            var args = new RoundStartedEventArgs(new Vector(directionX, directionY));
            ThreadContext.InvokeOnUiThread(() => this.OnRoundStarted(args));
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="score">The current score over all player rounds.</param>
        /// <param name="wasFinalRound">if set to <c>true</c> this was the final round and the game should be finished.</param>
        public void EndRound(int score, bool wasFinalRound)
        {
            var args = new RoundEndedEventArgs(score, wasFinalRound);
            ThreadContext.InvokeOnUiThread(() => this.OnRoundEnded(args));
        }

        /// <summary>
        /// Raises the <see cref="E:StartPointSet" /> event.
        /// </summary>
        /// <param name="args">The <see cref="BallPlacedOnGameFieldEventArgs"/> instance containing the event data.</param>
        private void OnStartPointSet(BallPlacedOnGameFieldEventArgs args)
        {
            this.StartPointSet(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:RoundStarted" /> event.
        /// </summary>
        /// <param name="args">The <see cref="RoundStartedEventArgs"/> instance containing the event data.</param>
        private void OnRoundStarted(RoundStartedEventArgs args)
        {
            this.RoundStarted(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:RoundEnded" /> event.
        /// </summary>
        /// <param name="args">The <see cref="RoundEndedEventArgs"/> instance containing the event data.</param>
        private void OnRoundEnded(RoundEndedEventArgs args)
        {
            this.RoundEnded(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:GameStarted" /> event.
        /// </summary>
        /// <param name="args">The <see cref="GameStartedEventArgs"/> instance containing the event data.</param>
        private void OnGameStarted(GameStartedEventArgs args)
        {
            this.GameStarted(this, args);
        }
    }
}
