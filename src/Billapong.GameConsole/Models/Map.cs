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

        /// <summary>
        /// Gets the windows grouped by rows.
        /// </summary>
        /// <value>
        /// The window rows.
        /// </value>
        public IDictionary<int, IEnumerable<Window>> Rows
        {
            get
            {
               return this.Windows.GroupBy(w => w.Y).OrderBy(w => w.First().Y).ToDictionary(w => w.Key, y => (IEnumerable<Window>)y.OrderBy(z => z.X).ToList());
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
