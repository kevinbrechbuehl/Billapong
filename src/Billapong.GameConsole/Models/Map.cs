namespace Billapong.GameConsole.Models
{
    using System.Collections.Generic;
    using System.Linq;

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

        public IDictionary<int, IEnumerable<Window>> Rows
        {
            get
            {
               return this.Windows.GroupBy(w => w.X).OrderBy(w => w.First().X).ToDictionary(w => w.Key, y => (IEnumerable<Window>)y.OrderBy(z => z.Y).ToList());
            }
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
