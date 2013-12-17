using System;

namespace Billapong.Core.Server.Tracing
{
    using System.Diagnostics;
    using Contract.Data.Tracing;
    using DataAccess.Repository;
    using LogMessage = DataAccess.Model.Tracing.LogMessage;

    /// <summary>
    /// The logger class.
    /// </summary>
    public class Logger
    {
        #region Singleton Implementation

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static Logger Current { get; private set; }

        /// <summary>
        /// Initializes the <see cref="Logger"/> class.
        /// </summary>
        static Logger()
        {
            Current = new Logger();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        private Logger()
        {
            this.repository = new Repository<LogMessage>();
        }

        #endregion

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository<LogMessage> repository;

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="component">The component.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public void LogMessage(DateTime timestamp, LogLevel logLevel, string component, string sender, string message)
        {
            var logMessage = new LogMessage
            {
                Timestamp = timestamp,
                LogLevel = logLevel.ToString(),
                Component = component,
                Sender = sender,
                Message = message
            };

            // save the log message to the database
            this.repository.Add(logMessage);
            this.repository.Save();

            // Also trace the message, to use this feature as well :)
            Trace.WriteLine(string.Format("{0} - {1} - {2}", timestamp, logLevel, message), string.Format("{0} ({1})", component, sender));
        }
    }
}
