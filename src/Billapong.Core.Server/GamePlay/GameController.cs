namespace Billapong.Core.Server.GamePlay
{
    using System;
    using System.Collections.Generic;
    using Contract.Service;

    public class GameController
    {
        #region Singleton Implementation

        /// <summary>
        /// Initializes the <see cref="GameController"/> class.
        /// </summary>
        static GameController()
        {
            Current = new GameController();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GameController"/> class from being created.
        /// </summary>
        private GameController()
        {
        }

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static GameController Current { get; private set; }

        #endregion

        private static object lockObject = new object();
        
        private IDictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        public Guid OpenGame(long mapId, IEnumerable<long> visibleWindows, string username, IGameConsoleCallback callback)
        {
            var game = new Game {Id = new Guid(), MapId = mapId};
            game.Callbacks.Add(callback);

            lock (lockObject)
            {
                games.Add(game.Id, game);
            }

            return game.Id;
        }

        public void JoinGame(Guid gameId, string username, IGameConsoleCallback callback)
        {
            Game game = null;
            lock (lockObject)
            {
                if (!games.ContainsKey(gameId))
                {
                    // todo (keb): schön machen
                    throw new Exception("game not open or not found");   
                }

                game = games[gameId];
            }

            game.Callbacks.Add(callback);
            this.StartGame(game);
        }

        private void StartGame(Game game)
        {
            foreach (var callback in game.Callbacks)
            {
                callback.StartGame(game.Id, "first", "second");
            }
        }
    }
}
