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
        [DataMember(Name = "WindowRows", Order = 1)]
        public int WindowRows { get; set; }

        /// <summary>
        /// Gets or sets the number of window cols.
        /// </summary>
        /// <value>
        /// The number of window cols.
        /// </value>
        [DataMember(Name = "WindowCols", Order = 1)]
        public int WindowCols { get; set; }

        /// <summary>
        /// Gets or sets the hole grid.
        /// </summary>
        /// <value>
        /// The hole grid.
        /// </value>
        [DataMember(Name = "HoleGrid", Order = 1)]
        public int HoleGrid { get; set; }
    }
}
