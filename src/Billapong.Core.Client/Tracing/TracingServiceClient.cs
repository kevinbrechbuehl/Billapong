namespace Billapong.Core.Client.Tracing
{
    using System.Collections.Generic;
    using Contract.Data.Tracing;
    using Contract.Service;

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
            this.ExecuteAsync(() => this.Proxy.Log(messages));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>
        /// Configuration based on server configuration for tracing.
        /// </returns>
        public TracingConfiguration GetConfig()
        {
            return this.Execute(() => this.Proxy.GetConfig());
        }
    }
}
