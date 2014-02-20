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
        /// The current round
        /// </summary>
        private int currentRound = 1;

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
        /// Gets a value indicating whether the current player starts the game.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current player starts the game; otherwise, <c>false</c>.
        /// </value>
        public bool StartGame { get; private set; }

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
        public int CurrentRound {
            get
            {
                return this.currentRound;
            }
            set
            {
                this.currentRound = value;
                OnPropertyChanged();
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
            this.StartGame = startGame;
            this.GameType = gameType;

            var localPlayer = new Player(Properties.Settings.Default.PlayerName, isGameOwner, true);
            this.LocalPlayer = localPlayer;
            if (!string.IsNullOrWhiteSpace(opponentName))
            {
                var opponent = new Player(opponentName, !isGameOwner, false);
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
