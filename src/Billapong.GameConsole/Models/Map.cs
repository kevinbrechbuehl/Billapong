namespace Billapong.GameConsole.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a map
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            this.Windows = new List<Window>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public IList<Window> Windows { get; private set; }
    }
}
