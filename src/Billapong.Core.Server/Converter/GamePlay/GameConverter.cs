namespace Billapong.Core.Server.Converter.GamePlay
{
    /// <summary>
    /// Convert game instances.
    /// </summary>
    public static class GameConverter
    {
        /// <summary>
        /// To the lobby contract.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Lobby game instance</returns>
        public static Contract.Data.GamePlay.LobbyGame ToLobbyContract(this Server.GamePlay.Game source)
        {
            return new Contract.Data.GamePlay.LobbyGame
            {
                Id = source.Id,
                Map = source.Map.Name,
                Username = source.Players[0].Name
            };
        }

        /// <summary>
        /// To the contract.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Contract game instance</returns>
        public static Contract.Data.GamePlay.Game ToContract(this Server.GamePlay.Game source)
        {
            var game = new Contract.Data.GamePlay.Game()
            {
                Id = source.Id,
                Map = source.Map.Name,
                Status = (Contract.Data.GamePlay.GameStatus)source.Status
            };

            if (source.Players != null && source.Players[0] != null)
            {
                game.Player1Name = source.Players[0].Name;
            }

            if (source.Players != null && source.Players[1] != null)
            {
                game.Player2Name = source.Players[1].Name;
            }

            return game;
        }
    }
}
