using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Billapong.Core.Client
{
    using System.Threading.Tasks;

    public class RichClientBase<TService>
    {
        protected TService Proxy { get; private set; }

        protected void Execute(Action delegatedAction)
        {
            try
            {
                this.ValidateProxy();
                delegatedAction();
            }
            catch (Exception)
            {
                // todo: handle this error -> i.e. when host is not running
                throw;
            }
        }

        protected void ExecuteAsync(Action delegatedAction)
        {
            Task.Factory.StartNew(() => this.Execute(delegatedAction));
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
                // todo: handle this error -> i.e. when host is not running
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
            this.Proxy = new ChannelFactory<TService>("*").CreateChannel();
        }
    }
}
