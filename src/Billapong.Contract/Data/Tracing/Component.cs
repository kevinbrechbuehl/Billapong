namespace Billapong.Contract.Data.Tracing
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Component enum.
    /// </summary>
    [DataContract(Name = "Component", Namespace = Globals.DataContractNamespaceName)]
    public enum Component
    {
        /// <summary>
        /// All components
        /// </summary>
        [EnumMember]
        All,
        
        /// <summary>
        /// The administration
        /// </summary>
        [EnumMember]
        Administration,

        /// <summary>
        /// The game console
        /// </summary>
        [EnumMember]
        GameConsole,

        /// <summary>
        /// The map editor
        /// </summary>
        [EnumMember]
        MapEditor,

        /// <summary>
        /// The server
        /// </summary>
        [EnumMember]
        Server
    }
}
