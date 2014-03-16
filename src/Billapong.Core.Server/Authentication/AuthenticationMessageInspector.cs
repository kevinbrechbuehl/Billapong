using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.Authentication
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Billapong.Contract;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;

    public class AuthenticationMessageInspector : IDispatchMessageInspector
    {
        private readonly Role role;

        public AuthenticationMessageInspector(Role role)
        {
            this.role = role;
        }
        
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (!this.IsSessionValid(this.GetSessionId(request)))
            {
                throw new InvalidSessionException("Invalid session id for current request");
            }

            return null;
        }

        private Guid GetSessionId(Message request)
        {
            var index = request.Headers.FindHeader(Globals.HeaderSessionIdKey, Globals.HeadersNamespaceName);
            return index != -1 ? request.Headers.GetHeader<Guid>(index) : Guid.Empty;
        }

        private bool IsSessionValid(Guid sessionId)
        {
            return SessionController.Current.IsValidSession(sessionId, this.role);
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}
