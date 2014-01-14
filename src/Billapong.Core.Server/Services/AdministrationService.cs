namespace Billapong.Core.Server.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Contract.Data.Tracing;
    using Contract.Service;
    using Converter.Tracing;
    using Tracing;

    /// <summary>
    /// Administration service implementation
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AdministrationService : IAdministrationService
    {
        /// <summary>
        /// Gets the log messages from the database.
        /// </summary>
        /// <param name="logListener">The log listener with the config which log messages to retrieve.</param>
        /// <returns>
        /// Log messages based on the listener configuration
        /// </returns>
        public IEnumerable<LogMessage> GetLogMessages(LogListener logListener)
        {
            // todo (kevin): This should only be possible if administrator role is authenticated
            return Logger.Current.GetLogMessages(logListener.LogLevel, logListener.Component, logListener.NumberOfMessages).Select(message => message.ToContract()).ToList();
        }
    }
}
