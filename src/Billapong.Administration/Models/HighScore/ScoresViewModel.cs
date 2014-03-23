namespace Billapong.Administration.Models.HighScore
{
    using System.Collections.Generic;

    /// <summary>
    /// Scores view model.
    /// </summary>
    public class ScoresViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the detail column.
        /// </summary>
        /// <value>
        ///   <c>true</c> if detail column should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDetailColumn { get; set; }

        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public IEnumerable<Contract.Data.Map.HighScore> Scores { get; set; }
    }
}