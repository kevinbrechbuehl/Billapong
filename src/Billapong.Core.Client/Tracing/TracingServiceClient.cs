namespace Billapong.Core.Client.Tracing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            this.Execute(() => this.Proxy.Log(messages));
        }

        /// <summary>
        /// Logs the specific messages async..
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>Async task</returns>
        public async Task LogAsync(IEnumerable<LogMessage> messages)
        {
            await this.ExecuteAsync(() => this.Proxy.Log(messages));
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

        /// <summary>
        /// Gets the configuration asynchronous.
        /// </summary>
        /// <returns>Async task for the configuration</returns>
        public async Task<TracingConfiguration> GetConfigAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetConfig());
        }
    }
}
