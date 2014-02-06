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
        public event EventHandler<BallPlacedOnGameFieldEventArgs> StartPointSet; 

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
        /// Raises the <see cref="E:GameStarted" /> event.
        /// </summary>
        /// <param name="args">The <see cref="GameStartedEventArgs"/> instance containing the event data.</param>
        private void OnGameStarted(GameStartedEventArgs args)
        {
            this.GameStarted(this, args);
        }

        public void GameError()
        {
            MessageBox.Show("Upps something went wrong, need to cancel the game...");
        }

        public void CancelGame()
        {
            MessageBox.Show("Someone/-thing has canceled the game...");
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The point x.</param>
        /// <param name="pointY">The point y.</param>
        public void SetStartPoint(long windowId, int pointX, int pointY)
        {
            var args = new BallPlacedOnGameFieldEventArgs(windowId, pointX, pointY);
            ThreadContext.InvokeOnUiThread(() => this.OnStartPointSet(args));   
        }

        /// <summary>
        /// Raises the <see cref="E:StartPointSet" /> event.
        /// </summary>
        /// <param name="args">The <see cref="BallPlacedOnGameFieldEventArgs"/> instance containing the event data.</param>
        private void OnStartPointSet(BallPlacedOnGameFieldEventArgs args)
        {
            this.StartPointSet(this, args);
        }

        public void StartRound(int pointX, int pointY)
        {
            throw new NotImplementedException();
        }

        public void EndRound(int score, bool wasFinalRound)
        {
            throw new NotImplementedException();
        }
    }
}
