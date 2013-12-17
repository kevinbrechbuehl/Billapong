using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Client.Tracing
{
    using System.ServiceModel;
    using Contract.Data.Tracing;

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
            // todo: in config auslagern
            this.proxy = new TracingServiceClient(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4710"));
        }

        #endregion

        private readonly TracingServiceClient proxy;

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

        private void Log(LogLevel logLevel, string message)
        {
            var messages = new List<LogMessage>();
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = logLevel, Message = message });
            this.proxy.Log(messages);
        }
    }
}
