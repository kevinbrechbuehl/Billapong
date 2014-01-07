namespace Billapong.Contract.Data.Tracing
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The log message data contract.
    /// </summary>
    [DataContract(Name = "LogMessage", Namespace = Globals.DataContractNamespaceName)]
    public class LogMessage
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
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [DataMember(Name = "Timestamp", Order = 1)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        [DataMember(Name = "Component", Order = 1)]
        public Component Component { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        [DataMember(Name = "Sender", Order = 1)]
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember(Name = "Message", Order = 1)]
        public string Message { get; set; }
    }
}
