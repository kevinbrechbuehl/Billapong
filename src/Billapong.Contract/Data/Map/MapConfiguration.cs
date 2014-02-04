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
        /// Gets or sets the number of rows.
        /// </summary>
        /// <value>
        /// The number of rows.
        /// </value>
        [DataMember(Name = "NumberOfRows", Order = 1)]
        public int NumberOfRows { get; set; }

        /// <summary>
        /// Gets or sets the number of cols.
        /// </summary>
        /// <value>
        /// The number of cols.
        /// </value>
        [DataMember(Name = "NumberOfCols", Order = 1)]
        public int NumberOfCols { get; set; }
    }
}
