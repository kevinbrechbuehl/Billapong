namespace Billapong.Core.Client.Tracing
{
    using Contract.Data.Tracing;
    using Contract.Service;
    using System.Collections.Generic;

    public class TracingServiceClient : RichClientBase<ITracingService>, ITracingService
    {
        public void Log(IEnumerable<LogMessage> messages)
        {
            base.ExecuteAsync(() => base.Proxy.Log(messages));
        }

        public TracingConfiguration GetConfig()
        {
            return base.Execute(() => base.Proxy.GetConfig());
        }
    }
}
