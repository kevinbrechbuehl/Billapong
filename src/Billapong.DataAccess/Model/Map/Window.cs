namespace Billapong.DataAccess.Model.Map
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a window in a map
    /// </summary>
    public class Window : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            this.Holes = new Collection<Hole>();
        }
        
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

        /// <summary>
        /// Gets or sets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public virtual ICollection<Hole> Holes { get; set; }
    }
}
