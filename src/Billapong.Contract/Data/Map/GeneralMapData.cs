namespace Billapong.Contract.Data.Map
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Data contract for general map.
    /// </summary>
    [DataContract(Name = "GeneralMapData", Namespace = Globals.DataContractNamespaceName)]
    public class GeneralMapData
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id", Order = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; }
    }
}
