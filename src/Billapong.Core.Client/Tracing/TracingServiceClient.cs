using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Client.Tracing
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Contract.Data.Tracing;
    using Contract.Service;

    public class TracingServiceClient : RichClientBase<ITracingService>, ITracingService
    {
        public TracingServiceClient(Binding binding, EndpointAddress endpointAddress) : base(binding, endpointAddress)
        {
        }

        public void Log(IEnumerable<LogMessage> messages)
        {
            base.ExecuteAsync(() => base.Proxy.Log(messages));
        }

        public TracingConfiguration GetConfig()
        {
            throw new NotImplementedException();
        }
    }
}
