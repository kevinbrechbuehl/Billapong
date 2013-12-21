namespace Billapong.Contract.Data.Editor
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The datacontract for a window.
    /// </summary>
    [DataContract(Name = "Window", Namespace = Globals.DataContractNamespaceName)]
    public class Window
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
        /// Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        [DataMember(Name = "X", Order = 1)]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        [DataMember(Name = "Y", Order = 1)]
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        [DataMember(Name = "Holes", Order = 1)]
        public IList<Hole> Holes { get; set; }
    }
}
