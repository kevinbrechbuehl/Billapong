namespace Billapong.DataAccess.Model.Editor
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The hole entity.
    /// </summary>
    public class Hole : IEntity
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
        /// Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        [Required]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        [Required]
        public int Y { get; set; }
    }
}
