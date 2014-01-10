using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Core.Server.GamePlay
{
    using Contract.Service;
    using DataAccess.Model.Map;

    public class Game
    {
        public Game()
        {
            this.Status = GameStatus.Open;
            this.Player1VisibleWindows = new List<long>();
            this.Player2VisibleWindows = new List<long>();
            this.Callbacks = new List<IGameConsoleCallback>();
        }
        
        public Guid Id { get; set; }
        
        public long MapId { get; set; }

        public Map Map { get; set; }

        public GameStatus Status { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public List<long> Player1VisibleWindows { get; private set; }

        public List<long> Player2VisibleWindows { get; private set; }

        public IList<IGameConsoleCallback> Callbacks { get; private set; } 
    }
}
