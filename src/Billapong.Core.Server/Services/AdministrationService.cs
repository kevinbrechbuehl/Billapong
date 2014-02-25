namespace Billapong.Core.Server.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Contract.Data.GamePlay;
    using Contract.Data.Map;
    using Contract.Data.Tracing;
    using Contract.Service;
    using Converter.GamePlay;
    using Converter.Map;
    using Converter.Tracing;
    using GamePlay;
    using Map;
    using Tracing;
    using Game = Contract.Data.GamePlay.Game;

    /// <summary>
    /// Administration service implementation
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AdministrationService : IAdministrationService
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
            // todo (breck1): This should only be possible if administrator role is authenticated
            return Logger.Current.GetLogMessages(logListener.LogLevel, logListener.Component, logListener.NumberOfMessages).Select(message => message.ToContract()).ToList();
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void ClearLog()
        {
            // todo (breck1): This should only be possible if administrator role is authenticated
            Logger.Current.ClearLog();
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <returns>All current available games on the server</returns>
        public IEnumerable<Game> GetGames()
        {
            // todo (breck1): This should only be possible if administrator role is authenticated
            return GameController.Current.GetAllGames().Select(game => game.ToContract());
        }

        /// <summary>
        /// Gets the map high scores.
        /// </summary>
        /// <returns>
        /// A highscore entry for each map with it's highest score
        /// </returns>
        public IEnumerable<HighScore> GetMapHighScores()
        {
            // todo (breck1): This should only be possible if administrator role is authenticated
            return MapController.Current.GetHighScores().Select(score => score.ToContract());
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
            // todo (breck1): This should only be possible if administrator role is authenticated
            return MapController.Current.GetHighScores(mapId).Select(score => score.ToContract());
        }
    }
}
