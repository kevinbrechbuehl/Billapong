namespace Billapong.Contract.Data.Tracing
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Listener class for the configuration of the log message retriever.
    /// </summary>
    [DataContract(Name = "LogListener", Namespace = Globals.DataContractNamespaceName)]
    public class LogListener
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
        /// Gets or sets the component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        [DataMember(Name = "Component", Order = 1)]
        public Component Component { get; set; }

        /// <summary>
        /// Gets or sets the number of messages.
        /// </summary>
        /// <value>
        /// The number of messages.
        /// </value>
        [DataMember(Name = "NumberOfMessages", Order = 1)]
        public int NumberOfMessages { get; set; }
    }
}
