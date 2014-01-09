using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.GamePlay
{
    using Contract.Service;

    public class Game
    {
        public Game()
        {
            this.Callbacks = new List<IGameConsoleCallback>();
        }
        
        public Guid Id { get; set; }
        
        public long MapId { get; set; }

        public IList<IGameConsoleCallback> Callbacks { get; private set; } 
    }
}
