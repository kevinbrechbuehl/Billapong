﻿namespace Billapong.DataAccess.Model.GamePlay
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity for highscores
    /// </summary>
    public class HighScore : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        [Required]
        public virtual Map.Map Map { get; set; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        [Required]
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        [Required]
        public long Score { get; set; }
    }
}
