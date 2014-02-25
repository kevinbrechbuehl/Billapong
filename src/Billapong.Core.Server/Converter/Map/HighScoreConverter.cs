namespace Billapong.Core.Server.Converter.Map
{
    using Contract.Data.Map;

    public static class HighScoreConverter
    {
        public static HighScore ToContract(this DataAccess.Model.Map.HighScore source)
        {
            return new HighScore
            {
                MapId = source.Map.Id,
                MapName = source.Map.Name,
                PlayerName = source.PlayerName,
                Score = source.Score
            };
        }
    }
}
