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
            this.Players = new Player[2]; // 2 player game
        }
        
        public Guid Id { get; set; }

        public Map Map { get; set; }

        public int BounceCount { get; set; }

        public GameStatus Status { get; set; }

        public Player[] Players { get; private set; }
    }
}
