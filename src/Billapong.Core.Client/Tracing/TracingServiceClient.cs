namespace Billapong.Core.Client.Tracing
{
    using Contract.Data.Tracing;
    using Contract.Service;
    using System.Collections.Generic;

    /// <summary>
    /// Service client for the tracing service
    /// </summary>
    public class TracingServiceClient : RichClientBase<ITracingService>, ITracingService
    {
        /// <summary>
        /// Logs the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Log(IEnumerable<LogMessage> messages)
        {
            base.ExecuteAsync(() => base.Proxy.Log(messages));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>
        /// Configuration based on server configuration for tracing.
        /// </returns>
        public TracingConfiguration GetConfig()
        {
            return base.Execute(() => base.Proxy.GetConfig());
        }
    }
}
