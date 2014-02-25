using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.Converter.GamePlay
{
    public static class HighScoreConverter
    {
        public static Contract.Data.GamePlay.HighScore ToContract(this DataAccess.Model.GamePlay.HighScore source)
        {
            return new Contract.Data.GamePlay.HighScore
            {
                MapId = source.Map.Id,
                MapName = source.Map.Name,
                PlayerName = source.PlayerName,
                Score = source.Score
            };
        }
    }
}
