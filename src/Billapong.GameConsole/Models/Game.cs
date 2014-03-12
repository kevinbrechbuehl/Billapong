namespace Billapong.GameConsole.Models
{
    using System;
    using System.Windows;
    using Configuration;
    using Core.Client.UI;

    /// <summary>
    /// Represents a specific game
    /// </summary>
    public class Game : NotificationObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            this.CurrentRound = 1;
        }

        /// <summary>
        /// Defines the possible game states
        /// </summary>
        public enum GameState
        {
            /// <summary>
            /// The game is running
            /// </summary>
            Running,

            /// <summary>
            /// The game ended normally
            /// </summary>
            Ended,

            /// <summary>
            /// The game got canceled
            /// </summary>
            Canceled
        }

        /// <summary>
        /// Gets the game identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        public Guid GameId { get; private set; }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public Map Map { get; private set; }

        /// <summary>
        /// Gets the local player.
        /// </summary>
        /// <value>
        /// The local player.
        /// </value>
        public Player LocalPlayer { get; private set; }

        /// <summary>
        /// Gets the opponent.
        /// </summary>
        /// <value>
        /// The opponent.
        /// </value>
        public Player Opponent { get; private set; }

        /// <summary>
        /// Gets the type of the game.
        /// </summary>
        /// <value>
        /// The type of the game.
        /// </value>
        public GameConfiguration.GameType GameType { get; private set; }

        /// <summary>
        /// Gets or sets the ball position.
        /// </summary>
        /// <value>
        /// The ball position.
        /// </value>
        public Point CurrentBallPosition { get; set; }

        /// <summary>
        /// Gets or sets the current round.
        /// </summary>
        /// <value>
        /// The current round.
        /// </value>
        public int CurrentRound 
        {
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
            } 
        }

        /// <summary>
        /// Gets or sets the current round score.
        /// </summary>
        /// <value>
        /// The current round score.
        /// </value>
        public int CurrentRoundScore { get; set; }

        /// <summary>
        /// Gets or sets the current window.
        /// </summary>
        /// <value>
        /// The current window.
        /// </value>
        public Window CurrentWindow { get; set; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        /// <value>
        /// The current player.
        /// </value>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Gets or sets the state of the current game.
        /// </summary>
        /// <value>
        /// The state of the current game.
        /// </value>
        public GameState CurrentGameState { get; set; }

        /// <summary>
        /// Initializes the specified game identifier.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="map">The map.</param>
        /// <param name="opponentName">Name of the opponent.</param>
        /// <param name="startGame">if set to <c>true</c> the current player starts the game.</param>
        /// <param name="isGameOwner">if set to <c>true</c> the current player owns the game.</param>
        /// <param name="gameType">Type of the game.</param>
        public void Init(Guid gameId, Map map, string opponentName, bool startGame, bool isGameOwner, GameConfiguration.GameType gameType)
        {
            this.GameId = gameId;
            this.Map = map;
            this.GameType = gameType;

            var localPlayer = new Player(Properties.Settings.Default.PlayerName, isGameOwner, true, startGame);
            this.LocalPlayer = localPlayer;
            if (!string.IsNullOrWhiteSpace(opponentName))
            {
                var opponent = new Player(opponentName, !isGameOwner, false, !startGame);
                this.Opponent = opponent;
                this.CurrentPlayer = startGame ? this.LocalPlayer : this.Opponent;
            }
            else
            {
                this.CurrentPlayer = this.LocalPlayer;
            }
        }
    }
}
