namespace Billapong.Core.Client
{
    using System.ServiceModel;

    public class CallbackClientBase<TService, TCallback> : RichClientBase<TService>, ICallback<TCallback>
    {
        public TCallback Callback { get; private set; }

        public CallbackClientBase(TCallback callback)
        {
            this.Callback = callback;
        }

        protected override TService CreateChannel()
        {
            return new DuplexChannelFactory<TService>(this.Callback, "*").CreateChannel();
        }
    }
}
