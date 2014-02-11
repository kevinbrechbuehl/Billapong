namespace Billapong.GameConsole.Models
{
    using System;
    using System.Windows;
    using Configuration;

    public class Game
    {
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
        /// Gets the name of the opponent.
        /// </summary>
        /// <value>
        /// The name of the opponent.
        /// </value>
        public string OpponentName { get; private set; }

        /// <summary>
        /// Gets the name of the own.
        /// </summary>
        /// <value>
        /// The name of the own.
        /// </value>
        public string OwnName { get; private set; }

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
        /// Gets or sets the current window.
        /// </summary>
        /// <value>
        /// The current window.
        /// </value>
        public Window CurrentWindow { get; set; }

        /// <summary>
        /// Initializes the specified game identifier.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="map">The map.</param>
        /// <param name="opponentName">Name of the opponent.</param>
        /// <param name="startGame">if set to <c>true</c> [start game].</param>
        /// <param name="gameType">Type of the game.</param>
        public void Init(Guid gameId, Map map, string opponentName, bool startGame, GameConfiguration.GameType gameType)
        {
            this.GameId = gameId;
            this.Map = map;
            this.OpponentName = opponentName;
            this.OwnName = Properties.Settings.Default.PlayerName;
            this.StartGame = startGame;
            this.GameType = gameType;
        }
    }
}
