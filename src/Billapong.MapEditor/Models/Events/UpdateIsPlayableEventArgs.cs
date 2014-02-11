using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models.Events
{
    public class UpdateIsPlayableEventArgs : EventArgs
    {
        public UpdateIsPlayableEventArgs(bool isPlayable)
        {
            this.IsPlayable = isPlayable;
        }

        public bool IsPlayable { get; private set; }
    }
}
