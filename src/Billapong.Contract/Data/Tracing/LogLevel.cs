namespace Billapong.Contract.Data.Tracing
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The enumeration for the log level
    /// </summary>
    [DataContract(Name = "LogLevel", Namespace = Globals.DataContractNamespaceName)]
    public enum LogLevel
    {
        /// <summary>
        /// The debug log level
        /// </summary>
        [EnumMember]
        Debug = 1,

        /// <summary>
        /// The information log level
        /// </summary>
        [EnumMember]
        Info = 2,

        /// <summary>
        /// The warn log level
        /// </summary>
        [EnumMember]
        Warn = 3,

        /// <summary>
        /// The error log level
        /// </summary>
        [EnumMember]
        Error = 4
    }
}
