namespace Billapong.Core.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Contract.Data.GamePlay;
    using Contract.Data.Map;
    using Contract.Service;
    using Converter.GamePlay;
    using Converter.Map;
    using GamePlay;
    using Map;

    /// <summary>
    /// The game console service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameConsoleService : IGameConsoleService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>
        /// Playable maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return MapController.Current.GetMaps(true).Select(map => map.ToContract()).ToList();
        }

        /// <summary>
        /// Gets the map by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The map, if valid map is found, null otheriwse
        /// </returns>
        public Map GetMapById(long id)
        {
            var map = MapController.Current.GetMapById(id, true);
            return map != null ? map.ToContract() : null;
        }

        /// <summary>
        /// Opens a new game and go into the lobby.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="visibleWindows">The visible windows.</param>
        /// <param name="username">The username.</param>
        /// <returns>
        /// Id of the game, used as correlation id
        /// </returns>
        public Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username)
        {
            return GameController.Current.OpenGame(mapId, visibleWindows, username, this.GetCallback());
        }

        /// <summary>
        /// Gets the lobby games.
        /// </summary>
        /// <returns>
        /// All open games in the lobby
        /// </returns>
        public IEnumerable<LobbyGame> GetLobbyGames()
        {
            return GameController.Current.GetOpenGames().Select(game => game.ToLobbyContract()).ToList();
        }

        /// <summary>
        /// Joins a game.
        /// </summary>
        /// <param name="gameId">The game identifier / correlation id of the game.</param>
        /// <param name="username">The username.</param>
        public void JoinGame(Guid gameId, string username)
        {
            GameController.Current.JoinGame(gameId, username, this.GetCallback());
        }

        /// <summary>
        /// Cancels a game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        public void CancelGame(Guid gameId)
        {
            GameController.Current.CancelGame(gameId);
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="windowId">The windows identifier.</param>
        /// <param name="pointX">The grid point x.</param>
        /// <param name="pointY">The grid point y.</param>
        public void SetStartPoint(Guid gameId, long windowId, double pointX, double pointY)
        {
            GameController.Current.SetStartPoint(gameId, windowId, pointX, pointY);
        }

        /// <summary>
        /// Starts the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="directionX">The direction x.</param>
        /// <param name="directionY">The point y where the user clicked to start the ball.</param>
        public void StartRound(Guid gameId, double directionX, double directionY)
        {
            GameController.Current.StartRound(gameId, directionX, directionY);
        }

        /// <summary>
        /// Adds the high score.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="playerName">Name of the player.</param>
        /// <param name="score">The score.</param>
        public void AddHighScore(long mapId, string playerName, int score)
        {
            GameController.Current.AddHighScore(mapId, playerName, score);
        }

        /// <summary>
        /// Ends the round.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="isPlayer1">if set to <c>true</c> user it player 1.</param>
        /// <param name="score">The score of the current round.</param>
        public void EndRound(Guid gameId, bool isPlayer1, int score)
        {
            GameController.Current.EndRound(gameId, isPlayer1, score);
        }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <returns>Callback channel for the current context</returns>
        private IGameConsoleCallback GetCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IGameConsoleCallback>();
        }
    }
}
