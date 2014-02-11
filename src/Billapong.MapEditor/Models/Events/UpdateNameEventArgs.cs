using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models.Events
{
    public class UpdateNameEventArgs : EventArgs
    {
        public UpdateNameEventArgs(string name)
        {
            this.Name = name;
        }
        
        public string Name { get; private set; }
    }
}
