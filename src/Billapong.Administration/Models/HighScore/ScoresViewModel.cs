namespace Billapong.Administration.Models.HighScore
{
    using System.Collections.Generic;

    public class ScoresViewModel
    {
        public bool ShowDetailColumn { get; set; }
        
        public IEnumerable<Contract.Data.Map.HighScore> Scores { get; set; }
    }
}