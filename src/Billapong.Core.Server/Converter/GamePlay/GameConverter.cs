namespace Billapong.Core.Server.Converter.GamePlay
{
    public static class GameConverter
    {
        public static Contract.Data.GamePlay.LobbyGame ToLobbyContract(this Server.GamePlay.Game source)
        {
            return new Contract.Data.GamePlay.LobbyGame
            {
                Id = source.Id,
                Map = source.Map.Name,
                Username = source.Players[0].Name
            };
        }

        public static Contract.Data.GamePlay.Game ToContract(this Server.GamePlay.Game source)
        {
            return new Contract.Data.GamePlay.Game()
            {
                Id = source.Id,
                Map = source.Map.Name,
                Player1Name = source.Players[0].Name,
                Player2Name = source.Players[1].Name,
                Status = (Contract.Data.GamePlay.GameStatus)source.Status
            };
        }
    }
}
