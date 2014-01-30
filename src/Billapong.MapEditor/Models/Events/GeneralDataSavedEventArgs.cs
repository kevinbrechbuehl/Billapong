using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models.Events
{
    public class GeneralDataSavedEventArgs : EventArgs
    {
        public GeneralDataSavedEventArgs(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        
        public long Id { get; private set; }
        
        public string Name { get; private set; }
    }
}
