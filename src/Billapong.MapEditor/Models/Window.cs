using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models
{
    using Core.Client.UI;

    public class Window : NotificationObject
    {
        public Window(int x, int y, int holeRows, int holeCols)
        {
            this.X = x;
            this.Y = y;
            this.Holes = new Hole[holeRows][];
            for (var i = 0; i < this.Holes.Length; i++)
            {
                this.Holes[i] = new Hole[holeCols];
                for (var j = 0; j < this.Holes[i].Length; j++)
                {
                    this.Holes[i][j] = new Hole(j, i);
                }
            }
        }
        
        public int X { get; private set; }

        public int Y { get; private set; }

        public Hole[][] Holes { get; private set; }
    }
}
