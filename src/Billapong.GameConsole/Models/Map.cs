using System.Collections.Generic;
namespace Billapong.GameConsole.Models
{
    public class Map
    {
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
        /// Gets or sets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public IList<Window> Windows { get; private set; }
    }
}
