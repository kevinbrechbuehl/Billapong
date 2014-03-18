namespace Billapong.Administration.Service
{
    using System;
    using System.Collections.Generic;
    using Billapong.Core.Client.Authentication;
    using Contract.Data.GamePlay;
    using Contract.Data.Map;
    using Contract.Data.Tracing;
    using Contract.Service;
    using Core.Client;

    /// <summary>
    /// Service client for the administration interface.
    /// </summary>
    public class AdministrationServiceClient : RichClientBase<IAdministrationService>, IAdministrationService
    {
        public AdministrationServiceClient(Guid sessionId)
            : base(new AuthenticationProvider(sessionId))
        {
            
        }
        
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

        /// <summary>
        /// Gets the map high scores.
        /// </summary>
        /// <returns>
        /// A highscore entry for each map with it's highest score
        /// </returns>
        public IEnumerable<HighScore> GetMapHighScores()
        {
            return this.Execute(() => this.Proxy.GetMapHighScores());
        }

        /// <summary>
        /// Gets the map scores.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>
        /// All score entries for a map
        /// </returns>
        public IEnumerable<HighScore> GetMapScores(long mapId)
        {
            return this.Execute(() => this.Proxy.GetMapScores(mapId));
        }
    }
}