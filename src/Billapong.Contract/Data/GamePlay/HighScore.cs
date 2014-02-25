namespace Billapong.Contract.Data.GamePlay
{
    using System.Runtime.Serialization;

    [DataContract(Name = "HighScore", Namespace = Globals.DataContractNamespaceName)]
    public class HighScore
    {
        /// <summary>
        /// Gets or sets the map identifier.
        /// </summary>
        /// <value>
        /// The map identifier.
        /// </value>
        [DataMember(Name = "MapId", Order = 1)]
        public long MapId { get; set; }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        [DataMember(Name = "MapName", Order = 1)]
        public string MapName { get; set; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        [DataMember(Name = "PlayerName", Order = 1)]
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        [DataMember(Name = "Score", Order = 1)]
        public int Score { get; set; }
    }
}
