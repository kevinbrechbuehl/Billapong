namespace Billapong.Core.Server.Converter.Map
{
    using Contract.Data.Map;

    /// <summary>
    /// Convert high scores objects
    /// </summary>
    public static class HighScoreConverter
    {
        /// <summary>
        /// To the contract.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Highscore contract instance</returns>
        public static HighScore ToContract(this DataAccess.Model.Map.HighScore source)
        {
            return new HighScore
            {
                MapId = source.Map.Id,
                MapName = source.Map.Name,
                PlayerName = source.PlayerName,
                Score = source.Score,
                Timestamp = source.Timestamp
            };
        }
    }
}
