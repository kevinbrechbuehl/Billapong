namespace Billapong.Contract.Service
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.GamePlay;
    using Data.Tracing;

    /// <summary>
    /// Service for administration console
    /// </summary>
    [ServiceContract(Name = "Administration", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IAdministrationService
    {
        /// <summary>
        /// Gets the log messages from the database.
        /// </summary>
        /// <param name="logListener">The log listener with the config which log messages to retrieve.</param>
        /// <returns>Log messages based on the listener configuration</returns>
        [OperationContract(Name = "GetLogMessages")]
        IEnumerable<LogMessage> GetLogMessages(LogListener logListener);

        /// <summary>
        /// Clears the log.
        /// </summary>
        [OperationContract(Name = "ClearLog")]
        void ClearLog();

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <returns>All current available games on the server</returns>
        [OperationContract(Name = "GetGames")]
        IEnumerable<Game> GetGames();

        /// <summary>
        /// Gets the map high scores.
        /// </summary>
        /// <returns>A highscore entry for each map with it's highest score</returns>
        [OperationContract(Name = "GetMapHighScores")]
        IEnumerable<HighScore> GetMapHighScores();

        /// <summary>
        /// Gets the map scores.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>All score entries for a map</returns>
        [OperationContract(Name = "GetMapScores")]
        IEnumerable<HighScore> GetMapScores(long mapId);
    }
}
