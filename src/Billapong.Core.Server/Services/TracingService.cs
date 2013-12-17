using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.Services
{
    using Contract.Data.Tracing;
    using Contract.Service;
    using Tracing;

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
