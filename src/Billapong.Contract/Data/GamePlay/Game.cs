namespace Billapong.Contract.Data.GamePlay
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The game contract.
    /// </summary>
    [DataContract(Name = "Game", Namespace = Globals.DataContractNamespaceName)]
    public class Game
    {
        /// <summary>
        /// Gets or sets thegame identifier.
        /// </summary>
        /// <value>
        /// The game identifier.
        /// </value>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the map name.
        /// </summary>
        /// <value>
        /// The map name.
        /// </value>
        [DataMember(Name = "Map", Order = 1)]
        public string Map { get; set; }

        /// <summary>
        /// Gets or sets the name of the player1.
        /// </summary>
        /// <value>
        /// The name of the player1.
        /// </value>
        [DataMember(Name = "Player1Name", Order = 1)]
        public string Player1Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the player2.
        /// </summary>
        /// <value>
        /// The name of the player2.
        /// </value>
        [DataMember(Name = "Player2Name", Order = 1)]
        public string Player2Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember(Name = "Status", Order = 1)]
        public GameStatus Status { get; set; }
    }
}
