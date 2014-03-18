namespace Billapong.Core.Server.Tracing
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using Billapong.Contract.Data.Tracing;

    /// <summary>
    /// Tracer class
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// The log level
        /// </summary>
        private static readonly LogLevel LogLevel;

        /// <summary>
        /// Initializes static members of the <see cref="Tracer" /> class.
        /// </summary>
        static Tracer()
        {
            if (!Enum.TryParse(ConfigurationManager.AppSettings["Tracing.LogLevel"], out LogLevel))
            {
                LogLevel = LogLevel.Debug;
            }
        }
        
        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message)
        {
            Trace.TraceInformation("DEBUG: {0}", message);
            Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            Trace.TraceInformation("INFO: {0}", message);
            Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(string message)
        {
            Trace.TraceWarning(message);
            Log(LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                message = string.Format("{0} - {1}{2}", message, exception.Message, exception.StackTrace);
            }

            Trace.TraceError(message);
            Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Logs the specified message to the database.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        private static void Log(LogLevel logLevel, string message)
        {
            if ((int)logLevel >= (int)LogLevel)
            {
                Logger.Current.LogMessage(DateTime.Now, logLevel, Component.Server, Component.Server.ToString(), message);
            }
        }
    }
}
