using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.GamePlay
{
    using Contract.Service;

    public class Player
    {
        public Player()
        {
            this.VisibleWindows = new List<long>();
        }
        
        public string Name { get; set; }

        public int Score { get; set; }

        public List<long> VisibleWindows { get; private set; }

        public IGameConsoleCallback Callback { get; set; }
    }
}
