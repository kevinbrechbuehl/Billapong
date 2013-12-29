namespace Billapong.Core.Client.Tracing
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Contract.Data.Tracing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Tracer implementation for the client.
    /// </summary>
    public class Tracer
    {
        #region Singleton Implementation

        private static readonly Tracer Current;

        static Tracer()
        {
            Current = new Tracer();
        }

        private Tracer()
        {
            this.proxy = new TracingServiceClient();
        }

        #endregion

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();
        
        /// <summary>
        /// The proxy
        /// </summary>
        private readonly TracingServiceClient proxy;

        /// <summary>
        /// The log messages saved for sending to the server when message retention count is reached
        /// </summary>
        private readonly IList<LogMessage> logMessages = new List<LogMessage>();

        /// <summary>
        /// The current component name
        /// </summary>
        private string component;

        /// <summary>
        /// The log level
        /// </summary>
        private LogLevel logLevel = 0;

        /// <summary>
        /// The message retention count
        /// </summary>
        private int messageRetentionCount = 0;

        /// <summary>
        /// Value indicated if the current Tracer has been initialized yet
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Initializes the tracer and load the configuration.
        /// </summary>
        /// <param name="component">The component name.</param>
        public static void Initialize(string component)
        {
            Current.InitializeConfig(component);
        }

        /// <summary>
        /// Shutdown the tracing. Send all messages in the queue to the server.
        /// </summary>
        public static void Shutdown()
        {
            Tracer.Info(string.Format("Shutdown tracing for component '{0}'", Current.component));
            Current.SendMessagesInQueue();
            Current.isInitialized = false;
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message)
        {
            Current.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            Current.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(string message)
        {
            Current.Log(LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                message = string.Format("{0} - {1}{2}", message, exception.Message, exception.StackTrace);
            }

            Current.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Gets the log messages.
        /// </summary>
        /// <param name="logLevel">The minimum log level.</param>
        /// <param name="component">The component to filter by.</param>
        /// <param name="numberOfMessages">The number of messages.</param>
        /// <returns>List of log messages</returns>
        public static IEnumerable<LogMessage> GetLogMessages(string component = "", LogLevel logLevel = LogLevel.Debug, int numberOfMessages = 0)
        {
            return Current.proxy.GetLogMessages(new LogListener
            {
                Component = component,
                LogLevel = logLevel,
                NumberOfMessages = numberOfMessages
            });
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="component">The component name.</param>
        private void InitializeConfig(string component)
        {
            this.component = component;

            Tracer.Debug(string.Format("Start initializing tracing for component '{0}'", component));

            // load the config async, so the client can start in this time
            Task.Factory.StartNew(() =>
            {
                var config = this.proxy.GetConfig();
                this.logLevel = config.LogLevel;
                this.messageRetentionCount = config.MessageRetentionCount;
                this.isInitialized = true;
                Tracer.Info(string.Format("Tracer for component '{0}' has been initialized", component));
            });
        }

        /// <summary>
        /// Logs the specified message
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        private void Log(LogLevel logLevel, string message)
        {
            if ((int)logLevel >= (int)this.logLevel)
            {
                lock (LockObject)
                {
                    this.logMessages.Add(new LogMessage
                    {
                        Timestamp = DateTime.Now,
                        Component = this.component,
                        Sender = Environment.MachineName,
                        LogLevel = logLevel,
                        Message = message
                    });
                }

                // only send to the server if message retention count is reached
                if (this.logMessages.Count >= this.messageRetentionCount)
                {
                    this.SendMessagesInQueue();
                }
            }
        }

        /// <summary>
        /// Sends the messages in the queue to the server.
        /// </summary>
        private void SendMessagesInQueue()
        {
            lock (LockObject)
            {
                // warn because tracer has not yet been initialized
                if (!this.isInitialized)
                {
                    Trace.TraceWarning("Tracer has not yet been initialized, so messages could not be sent.");
                    return;
                }

                this.proxy.Log(this.logMessages.ToList());
                this.logMessages.Clear();
            }
        }
    }
}
