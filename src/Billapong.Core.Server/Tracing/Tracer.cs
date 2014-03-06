using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.Tracing
{
    using System.Configuration;
    using System.Diagnostics;

    using Billapong.Contract.Data.Tracing;

    public class Tracer
    {
        private static LogLevel LogLevel;
        
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

        private static void Log(LogLevel logLevel, string message)
        {
            if ((int)logLevel >= (int)LogLevel)
            {
                Logger.Current.LogMessage(DateTime.Now, logLevel, Component.Server, Component.Server.ToString(), message);
            }
        }
    }
}
