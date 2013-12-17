namespace Billapong.Core.Server.Services
{
    using Contract.Data.Tracing;
    using Contract.Service;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Tracing;

    /// <summary>
    /// The tracing service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TracingService : ITracingService
    {
        /// <summary>
        /// Logs the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Log(IEnumerable<LogMessage> messages)
        {
            foreach (var message in messages)
            {
                Logger.Current.LogMessage(message.Timestamp, message.LogLevel, message.Component, message.Sender, message.Message);
            }
        }

        public TracingConfiguration GetConfig()
        {
            throw new NotImplementedException();
        }
    }
}
