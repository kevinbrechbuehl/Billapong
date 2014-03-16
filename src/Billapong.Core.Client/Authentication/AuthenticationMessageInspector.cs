using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Client.Authentication
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Billapong.Contract;

    public class AuthenticationMessageInspector : IClientMessageInspector
    {
        private readonly Guid sessionId;

        public AuthenticationMessageInspector(Guid sessionId)
        {
            this.sessionId = sessionId;
        }
        
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (this.sessionId != Guid.Empty)
            {
                request.Headers.Add(MessageHeader.CreateHeader(Globals.HeaderSessionIdKey, Globals.HeadersNamespaceName, this.sessionId));
            }

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}
