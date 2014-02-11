using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models.Events
{
    public class GameHoleClickedEventArgs : EventArgs
    {
        public GameHoleClickedEventArgs(long windowId, int windowX, int windowY, long holeId, int holeX = 0, int holeY = 0)
        {
            this.HoleId = holeId;
            this.HoleX = holeX;
            this.HoleY = holeY;
            this.WindowId = windowId;
            this.WindowX = windowX;
            this.WindowY = windowY;
        }
        
        public long HoleId { get; private set; }

        public int HoleX { get; private set; }

        public int HoleY { get; private set; }

        public long WindowId { get; private set; }

        public int WindowX { get; private set; }

        public int WindowY { get; private set; }
    }
}
