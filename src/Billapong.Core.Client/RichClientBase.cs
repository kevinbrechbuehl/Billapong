using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Billapong.Core.Client
{
    public class RichClientBase<TService>
    {
        protected TService Proxy { get; private set; }

        private Binding binding;

        private EndpointAddress endpointAddress;

        public RichClientBase(Binding binding, EndpointAddress endpointAddress)
        {
            // todo: refactore

            // todo: implement idisposable
            
            this.binding = binding;
            this.endpointAddress = endpointAddress;
        }

        protected void Execute(Action delegatedAction)
        {
            try
            {
                this.ValidateProxy();
                delegatedAction();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected TResult Execute<TResult>(Func<TResult> delegatedFunction)
        {
            try
            {
                this.ValidateProxy();
                return delegatedFunction();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ValidateProxy()
        {
            if (this.Proxy == null)
            {
                CreateProxy();
            }
            else if(((IChannel)this.Proxy).State == CommunicationState.Opened)
            {
                return;
            }
            else if (((IChannel)this.Proxy).State == CommunicationState.Faulted)
            {
                ((IChannel)this.Proxy).Abort();
                this.CreateProxy();
            }
            else
            {
                throw new InvalidOperationException("Validate state of proxy failed.");
            }
        }

        private void CreateProxy()
        {
            this.Proxy = ChannelFactory<TService>.CreateChannel(this.binding, this.endpointAddress);
        }
    }
}
