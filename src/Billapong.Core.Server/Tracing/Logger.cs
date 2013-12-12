using System;

namespace Billapong.Core.Server.Tracing
{
    using Contract.Data.Tracing;
    using System.Diagnostics;

    public class Logger
    {
        public static void LogMessage(DateTime timestamp, LogLevel logLevel, string module, string message)
        {
            switch (logLevel)
            {
                 case LogLevel.Debug:
                    WriteLog(timestamp, "DEBUG", module, message);
                    break;
                 case LogLevel.Info:
                    WriteLog(timestamp, "INFO", module, message);
                    break;
                 case LogLevel.Warn:
                    WriteLog(timestamp, "WARN", module, message);
                    break;
                 case LogLevel.Error:
                    WriteLog(timestamp, "ERROR", module, message);
                    break;
            }
        }

        private static void WriteLog(DateTime timestamp, string logLevel, string module, string message)
        {
            Trace.WriteLine(string.Format("{0} - {1} - {2} :: {3}", timestamp, logLevel, module, message));
        }
    }
}
