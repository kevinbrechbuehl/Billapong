using System;

namespace Billapong.Core.Server.Tracing
{
    using System.Diagnostics;
    using Contract.Data.Tracing;
    using DataAccess.Repository;
    using LogMessage = DataAccess.Model.Tracing.LogMessage;

    public class Logger
    {
        #region Singleton Implementation

        public static Logger Instance { get; private set; }

        static Logger()
        {
            Instance = new Logger();
        }

        private Logger()
        {
            this.repository = new Repository<LogMessage>();
        }

        #endregion

        private readonly IRepository<LogMessage> repository; 
        
        public void LogMessage(DateTime timestamp, LogLevel logLevel, string module, string message)
        {
            var logMessage = new LogMessage
            {
                Timestamp = timestamp,
                LogLevel = logLevel.ToString(),
                Module = module,
                Message = message
            };

            this.repository.Add(logMessage);
            this.repository.Save();

            Trace.WriteLine(string.Format("{0} - {1} - {2}", timestamp, logLevel, message), module);
        }
    }
}
