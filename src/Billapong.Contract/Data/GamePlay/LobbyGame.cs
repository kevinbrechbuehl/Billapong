namespace Billapong.Contract.Data.GamePlay
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The lobby game contract.
    /// </summary>
    [DataContract(Name = "LobbyGame", Namespace = Globals.DataContractNamespaceName)]
    public class LobbyGame
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
        /// Gets or sets the username of the game owner.
        /// </summary>
        /// <value>
        /// The game owner username.
        /// </value>
        [DataMember(Name = "Username", Order = 1)]
        public string Username { get; set; }
    }
}
