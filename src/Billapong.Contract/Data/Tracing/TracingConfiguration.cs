namespace Billapong.Contract.Data.Tracing
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tracing configuration data contract.
    /// </summary>
    [DataContract(Name = "TracingConfiguration", Namespace = Globals.DataContractNamespaceName)]
    public class TracingConfiguration
    {
        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        [DataMember(Name = "LogLevel", Order = 1)]
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the message retention count.
        /// </summary>
        /// <value>
        /// The message retention count.
        /// </value>
        [DataMember(Name = "MessageRetentionCount", Order = 1)]
        public int MessageRetentionCount { get; set; }
    }
}
