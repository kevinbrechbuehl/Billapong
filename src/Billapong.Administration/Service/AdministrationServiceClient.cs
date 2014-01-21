namespace Billapong.Administration.Service
{
    using System.Collections.Generic;
    using Contract.Data.GamePlay;
    using Contract.Data.Tracing;
    using Contract.Service;
    using Core.Client;

    /// <summary>
    /// Service client for the administration interface.
    /// </summary>
    public class AdministrationServiceClient : RichClientBase<IAdministrationService>, IAdministrationService
    {
        /// <summary>
        /// Gets the log messages from the database.
        /// </summary>
        /// <param name="logListener">The log listener with the config which log messages to retrieve.</param>
        /// <returns>
        /// Log messages based on the listener configuration
        /// </returns>
        public IEnumerable<LogMessage> GetLogMessages(LogListener logListener)
        {
            return this.Execute(() => this.Proxy.GetLogMessages(logListener));
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void ClearLog()
        {
            this.Execute(() => this.Proxy.ClearLog());
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <returns>All current available games on the server</returns>
        public IEnumerable<Game> GetGames()
        {
            return this.Execute(() => this.Proxy.GetGames());
        }
    }
}