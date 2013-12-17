namespace Billapong.Core.Server.Services
{
    using Contract.Data.Tracing;
    using Contract.Service;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Tracing;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TracingService : ITracingService
    {
        public void Log(IEnumerable<LogMessage> messages)
        {
            foreach (var message in messages)
            {
                Logger.Instance.LogMessage(message.Timestamp, message.LogLevel, message.Component, message.Sender, message.Message);
            }
        }

        public TracingConfiguration GetConfig()
        {
            throw new NotImplementedException();
        }
    }
}
