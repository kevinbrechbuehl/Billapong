namespace Billapong.Core.Client
{
    using System.ServiceModel;

    /// <summary>
    /// Base implementation for a callback service client
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TCallback">The type of the callback.</typeparam>
    public class CallbackClientBase<TService, TCallback> : RichClientBase<TService>, ICallback<TCallback>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackClientBase{TService, TCallback}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public CallbackClientBase(TCallback callback)
        {
            this.Callback = callback;
        }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        public TCallback Callback { get; private set; }

        /// <summary>
        /// Creates the channel.
        /// </summary>
        /// <returns>The client.</returns>
        protected override TService CreateChannel()
        {
            return new DuplexChannelFactory<TService>(this.Callback, "*").CreateChannel();
        }
    }
}
