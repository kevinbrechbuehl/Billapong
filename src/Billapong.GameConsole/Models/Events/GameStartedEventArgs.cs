namespace Billapong.GameConsole.Models.Events
{
    using System;

    /// <summary>
    /// The arguments of the GameStarted event
    /// </summary>
    public class GameStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStartedEventArgs" /> class.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="map">The map.</param>
        /// <param name="opponent">The opponent.</param>
        /// <param name="startGame">if set to <c>true</c> [start game].</param>
        public GameStartedEventArgs(Guid gameId, Map map, string opponent, bool startGame)
        {
            this.GameId = gameId;
            this.Map = map;
            this.Opponent = opponent;
            this.StartGame = startGame;
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
        /// Gets the opponent.
        /// </summary>
        /// <value>
        /// The opponent.
        /// </value>
        public string Opponent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current player will start first round.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current player starts the first round; otherwise, <c>false</c>.
        /// </value>
        public bool StartGame { get; private set; }
    }
}