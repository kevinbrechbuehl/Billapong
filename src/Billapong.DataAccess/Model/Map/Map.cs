namespace Billapong.DataAccess.Model.Map
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a map
    /// </summary>
    public class Map : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            this.Windows = new Collection<Window>();
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this map is playable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the map is playable; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public virtual ICollection<Window> Windows { get; set; }
    }
}
