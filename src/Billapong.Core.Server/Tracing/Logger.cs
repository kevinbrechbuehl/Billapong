namespace Billapong.Core.Server.Tracing
{
    using Contract.Data.Tracing;
    using DataAccess.Repository;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
        public void LogMessage(DateTime timestamp, LogLevel logLevel, Component component, string sender, string message)
        {
            var logMessage = new LogMessage
            {
                Timestamp = timestamp,
                LogLevel = (int)logLevel,
                Component = component.ToString(),
                Sender = sender,
                Message = message
            };

            // save the log message to the database
            this.repository.Add(logMessage);
            this.repository.Save();

            // Also trace the message, to use this feature as well :)
            Trace.WriteLine(string.Format("{0} - {1} - {2}", timestamp, logLevel, message), string.Format("{0} ({1})", component, sender));
        }

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <param name="logLevel">The minimum log level.</param>
        /// <param name="component">The component to filter by.</param>
        /// <param name="numberOfMessages">The number of messages.</param>
        /// <returns>List of log messages</returns>
        public IEnumerable<LogMessage> GetLogMessages(LogLevel logLevel, Component component, int numberOfMessages)
        {
            var messages = this.repository.Get(filter: message => message.LogLevel >= (int)logLevel);

            // filter component
            if (component != Component.All)
            {
                messages = messages.Where(message => message.Component == component.ToString());
            }

            // sort descending by date
            messages = messages.OrderByDescending(message => message.Timestamp);

            // get only specific number of entries
            if (numberOfMessages > 0)
            {
                messages = messages.Take(numberOfMessages);
            }

            return messages;
        }
    }
}
