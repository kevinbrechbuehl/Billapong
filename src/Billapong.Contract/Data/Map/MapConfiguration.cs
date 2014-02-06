namespace Billapong.Contract.Data.Map
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Map configuration contract
    /// </summary>
    [DataContract(Name = "MapConfiguration", Namespace = Globals.DataContractNamespaceName)]
    public class MapConfiguration
    {
        /// <summary>
        /// Gets or sets the number of window rows.
        /// </summary>
        /// <value>
        /// The number of window rows.
        /// </value>
        [DataMember(Name = "NumberOfWindowRows", Order = 1)]
        public int NumberOfWindowRows { get; set; }

        /// <summary>
        /// Gets or sets the number of window cols.
        /// </summary>
        /// <value>
        /// The number of window cols.
        /// </value>
        [DataMember(Name = "NumberOfWindowCols", Order = 1)]
        public int NumberOfWindowCols { get; set; }

        /// <summary>
        /// Gets or sets the number of hole rows.
        /// </summary>
        /// <value>
        /// The number of hole rows.
        /// </value>
        [DataMember(Name = "NumberOfHoleRows", Order = 1)]
        public int NumberOfHoleRows { get; set; }

        /// <summary>
        /// Gets or sets the number of hole cols.
        /// </summary>
        /// <value>
        /// The number of hole cols.
        /// </value>
        [DataMember(Name = "NumberOfHoleCols", Order = 1)]
        public int NumberOfHoleCols { get; set; }
    }
}
