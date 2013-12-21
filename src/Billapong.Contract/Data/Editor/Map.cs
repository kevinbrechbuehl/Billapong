namespace Billapong.Contract.Data.Editor
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The datacontract for a map.
    /// </summary>
    [DataContract(Name = "Map", Namespace = Globals.DataContractNamespaceName)]
    public class Map
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

        /// <summary>
        /// Gets or sets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        [DataMember(Name = "Windows", Order = 1)]
        public IList<Window> Windows { get; set; }
    }
}
