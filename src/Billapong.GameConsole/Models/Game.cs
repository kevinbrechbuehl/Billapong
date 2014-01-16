namespace Billapong.GameConsole.Models
{
    using System;

    public class Game
    {
        public Guid GameId { get; private set; }

        public Map Map { get; private set; }

        public string OpponentName { get; private set; }

        public string OwnName { get; private set; }

        public bool StartGame { get; private set; }

        public void Init(Guid gameId, Map map, string opponentName, bool startGame)
        {
            this.GameId = gameId;
            this.Map = map;
            this.OpponentName = opponentName;
            this.OwnName = Properties.Settings.Default.PlayerName;
            this.StartGame = startGame;
        }
    }
}
