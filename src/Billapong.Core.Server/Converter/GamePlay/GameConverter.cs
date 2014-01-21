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
                Username = source.Player1Name
            };
        }

        public static Contract.Data.GamePlay.Game ToContract(this Server.GamePlay.Game source)
        {
            return new Contract.Data.GamePlay.Game()
            {
                Id = source.Id,
                Map = source.Map.Name,
                Player1Name = source.Player1Name,
                Player2Name = source.Player2Name,
                Status = (Contract.Data.GamePlay.GameStatus)source.Status
            };
        }
    }
}
