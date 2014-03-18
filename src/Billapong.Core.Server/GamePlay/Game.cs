namespace Billapong.Core.Server.GamePlay
{
    using System;
    using DataAccess.Model.Map;

    /// <summary>
    /// Model for a game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            this.Status = GameStatus.Open;
            this.Players = new Player[2]; // 2 player game
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public Map Map { get; set; }

        /// <summary>
        /// Gets or sets the bounce count.
        /// </summary>
        /// <value>
        /// The bounce count.
        /// </value>
        public int BounceCount { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public GameStatus Status { get; set; }

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public Player[] Players { get; private set; }
    }
}
