namespace Billapong.Contract.Exceptions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for invalid map.
    /// </summary>
    [DataContract(Name = "MapNotFoundException", Namespace = Globals.ExceptionContractNamespaceName)]
    public class MapNotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapNotFoundException"/> class.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public MapNotFoundException(long mapId)
        {
            this.MapId = mapId;
            this.Message = string.Format("Map with id '{0}' not found.", mapId);
        }
        
        /// <summary>
        /// Gets or sets the map identifier.
        /// </summary>
        /// <value>
        /// The map identifier.
        /// </value>
        [DataMember(Name = "MapId", Order = 1)]
        public long MapId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }
    }
}
