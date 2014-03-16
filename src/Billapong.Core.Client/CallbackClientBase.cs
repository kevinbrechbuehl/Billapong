namespace Billapong.Core.Client
{
    using System.ServiceModel;
    using Billapong.Core.Client.Authentication;

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
        /// <param name="authenticationProvider">The authentication provider if login is needed</param>
        public CallbackClientBase(TCallback callback, AuthenticationProvider authenticationProvider = null) : base(authenticationProvider)
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
        /// Creates the channel factory.
        /// </summary>
        /// <returns>Channgel factory of type TService</returns>
        protected override ChannelFactory<TService> CreateChannelFactory()
        {
            return new DuplexChannelFactory<TService>(this.Callback, "*");
        }
    }
}
