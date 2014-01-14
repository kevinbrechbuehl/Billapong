namespace Billapong.Core.Server.Converter.GamePlay
{
    public static class GameConverter
    {
        public static Contract.Data.GamePlay.LobbyGame ToContract(this Server.GamePlay.Game source)
        {
            return new Contract.Data.GamePlay.LobbyGame
            {
                Id = source.Id,
                Map = source.Map.Name,
                Username = source.Player1Name
            };
        }
    }
}
