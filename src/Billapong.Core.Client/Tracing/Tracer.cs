namespace Billapong.Core.Client.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using Billapong.Core.Client.Exceptions;
    using Contract.Data.Tracing;
    using Component = Contract.Data.Tracing.Component;

    /// <summary>
    /// The Tracer implementation for the client.
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// The Tracer instance
        /// </summary>
        private static readonly Tracer Current;

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
        private Component component;

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

        #region Singleton Implementation

        /// <summary>
        /// Initializes static members of the <see cref="Tracer"/> class.
        /// </summary>
        static Tracer()
        {
            Current = new Tracer();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Tracer"/> class from being created.
        /// </summary>
        private Tracer()
        {
            this.proxy = new TracingServiceClient();
        }

        #endregion

        /// <summary>
        /// Initializes the tracer and load the configuration.
        /// </summary>
        /// <param name="component">The component name.</param>
        public static async Task Initialize(Component component)
        {
            await Current.InitializeConfig(component);
        }

        /// <summary>
        /// Shutdown the tracing. Send all messages in the queue to the server.
        /// </summary>
        public static async Task Shutdown()
        {
            Tracer.Info(string.Format("Shutdown tracing for component '{0}'", Current.component));
            await Current.SendMessagesInQueue();
            Current.isInitialized = false;
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message.</param>
        public static async Task Debug(string message)
        {
            Trace.TraceInformation("DEBUG: {0}", message);
            await Current.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message.</param>
        public static async Task Info(string message)
        {
            Trace.TraceInformation("INFO: {0}", message);
            await Current.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message.</param>
        public static async Task Warn(string message)
        {
            Trace.TraceWarning(message);
            await Current.Log(LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static async Task Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                message = string.Format("{0} - {1}{2}", message, exception.Message, exception.StackTrace);
            }

            Trace.TraceError(message);
            await Current.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Processes the queued messages.
        /// </summary>
        public static async Task ProcessQueuedMessages()
        {
            await Current.SendMessagesInQueue();
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="component">The component name.</param>
        private async Task InitializeConfig(Component component)
        {
            this.component = component;

            await Tracer.Debug(string.Format("Start initializing tracing for component '{0}'", component));

            // load the config async, so the client can start in this time
            var config = await this.proxy.GetConfigAsync();
            this.logLevel = config.LogLevel;
            this.messageRetentionCount = config.MessageRetentionCount;
            this.isInitialized = true;
            await Tracer.Info(string.Format("Tracer for component '{0}' has been initialized", component));
        }

        /// <summary>
        /// Logs the specified message
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        private async Task Log(LogLevel logLevel, string message)
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
                    await this.SendMessagesInQueue();
                }
            }
        }

        /// <summary>
        /// Sends the messages in the queue to the server.
        /// </summary>
        private async Task SendMessagesInQueue()
        {
            var messages = new List<LogMessage>();
            
            lock (LockObject)
            {
                // warn because tracer has not yet been initialized
                if (!this.isInitialized)
                {
                    Trace.TraceWarning("Tracer has not yet been initialized, so messages could not be sent.");
                    return;
                }

                messages.AddRange(this.logMessages);
                this.logMessages.Clear();
            }

            try
            {
                await this.proxy.LogAsync(messages);
            }
            catch (ServerUnavailableException ex)
            {
                Trace.TraceError("Server not available: " + ex.Message);
            }
        }
    }
}
