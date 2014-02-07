using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models
{
    public class Map
    {
        public long Id { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public bool IsPlayable { get; set; }

        public int NumberOfWindows { get; set; }

        public int NumberOfHoles { get; set; }

        public IList<Contract.Data.Map.Window> Windows { get; set; }
    }
}
