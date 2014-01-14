namespace Billapong.Core.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using Billapong.Contract.Data.Tracing;
    using Billapong.Contract.Service;
    using Billapong.Core.Server.Converter.Tracing;
    using Tracing;

    /// <summary>
    /// The tracing service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TracingService : ITracingService
    {
        /// <summary>
        /// Logs the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Log(IEnumerable<LogMessage> messages)
        {
            var config = this.GetConfig();
            foreach (var message in messages.Where(message => (int)message.LogLevel >= (int)config.LogLevel))
            {
                Logger.Current.LogMessage(message.Timestamp, message.LogLevel, message.Component, message.Sender, message.Message);
            }
        }

        /// <summary>
        /// Gets the tracing configuration.
        /// </summary>
        /// <returns>
        /// Configuration based on server configuration for tracing.
        /// </returns>
        public TracingConfiguration GetConfig()
        {
            LogLevel logLevel;
            if (!Enum.TryParse(ConfigurationManager.AppSettings["Tracing.LogLevel"], out logLevel))
            {
                logLevel = LogLevel.Debug;
            }

            int messageRetentionCount;
            if (!int.TryParse(ConfigurationManager.AppSettings["Tracing.MessageRetentionCount"], out messageRetentionCount))
            {
                messageRetentionCount = 100;
            }

            return new TracingConfiguration { LogLevel = logLevel, MessageRetentionCount = messageRetentionCount };
        }

        /// <summary>
        /// Gets the log messages from the database.
        /// </summary>
        /// <param name="logListener">The log listener with the config which log messages to retrieve.</param>
        /// <returns>
        /// Log messages based on the listener configuration
        /// </returns>
        public IEnumerable<LogMessage> GetLogMessages(LogListener logListener)
        {
            // todo (kevin): This should only be possible if administrator role is authenticated -> move to AdministrationService to make this possible
            return Logger.Current.GetLogMessages(logListener.LogLevel, logListener.Component, logListener.NumberOfMessages).Select(message => message.ToContract()).ToList();
        }
    }
}
