using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models.Events
{
    public class GameWindowEventArgs : EventArgs
    {
        public GameWindowEventArgs(long id, int x = 0, int y = 0)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }
        
        public long Id { get; private set; }
        
        public int X { get; private set; }

        public int Y { get; private set; }
    }
}
