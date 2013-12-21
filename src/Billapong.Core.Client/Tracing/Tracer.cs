namespace Billapong.Core.Client.Tracing
{
    using Contract.Data.Tracing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

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

        private readonly TracingServiceClient proxy;

        private readonly object lockObject = new object();

        private readonly IList<LogMessage> logMessages = new List<LogMessage>();  
        
        private string component;

        private bool isInitialized;

        private LogLevel logLevel = 0;

        private int messageRetentionCount;

        public static void Initialize(string component)
        {
            Current.InitializeConfig(component);
        }

        public static void Debug(string message)
        {
            Current.Log(LogLevel.Debug, message);
        }

        public static void Info(string message)
        {
            Current.Log(LogLevel.Info, message);
        }

        public static void Warn(string message)
        {
            Current.Log(LogLevel.Warn, message);
        }

        public static void Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                message = string.Format("{0} - {1}{2}", message, exception.Message, exception.StackTrace);
            }

            Current.Log(LogLevel.Error, message);
        }

        private void InitializeConfig(string component)
        {
            this.component = component;

            Task.Factory.StartNew(() =>
            {
                var config = this.proxy.GetConfig();
                this.logLevel = config.LogLevel;
                this.messageRetentionCount = config.MessageRetentionCount;
                this.isInitialized = true;
            });
        }

        private void Log(LogLevel logLevel, string message)
        {
            if (!this.isInitialized)
            {
                Trace.TraceWarning("Tracer has not yet been initialized");
                return;
            }

            if ((int)logLevel >= (int)this.logLevel)
            {
                lock (this.lockObject)
                {
                    this.logMessages.Add(new LogMessage
                    {
                        Timestamp = DateTime.Now,
                        Component = this.component,
                        Sender = Environment.MachineName,
                        LogLevel = logLevel,
                        Message = message
                    });

                    if (this.logMessages.Count >= this.messageRetentionCount)
                    {
                        this.proxy.Log(this.logMessages.ToList());
                        this.logMessages.Clear();
                    }
                }
            }
        }
    }
}
