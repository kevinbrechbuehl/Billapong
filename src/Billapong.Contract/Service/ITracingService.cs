namespace Billapong.Contract.Service
{
    using Data.Tracing;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// The tracing service.
    /// </summary>
    [ServiceContract(Name = "Tracing", Namespace = Globals.ServiceContractNamespaceName)]
    public interface ITracingService
    {
        /// <summary>
        /// Logs the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        [OperationContract(Name = "Log", IsOneWay = true)]
        void Log(IEnumerable<LogMessage> messages);

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>Configuration based on server configuration for tracing.</returns>
        [OperationContract(Name = "GetConfig")]
        TracingConfiguration GetConfig();

        /// <summary>
        /// Gets the log messages from the database.
        /// </summary>
        /// <param name="logListener">The log listener with the config which log messages to retrieve.</param>
        /// <returns>Log messages based on the listener configuration</returns>
        [OperationContract(Name = "GetLogMessages")]
        IEnumerable<LogMessage> GetLogMessages(LogListener logListener);
    }
}
