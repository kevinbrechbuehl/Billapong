namespace Billapong.GameConsole.Service
{
    /// <summary>
    /// Provides a static context for the gameconsole
    /// </summary>
    public class GameConsoleContext
    {
        #region Singleton implementation

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly GameConsoleContext Instance = new GameConsoleContext();

        /// <summary>
        /// Initializes the <see cref="GameConsoleContext"/> class.
        /// </summary>
        static GameConsoleContext()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GameConsoleContext"/> class from being created.
        /// </summary>
        private GameConsoleContext()
        {
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static GameConsoleContext Current
        {
            get
            {
                return Instance;
            }
        }

        #endregion

        /// <summary>
        /// The game console service client
        /// </summary>
        private GameConsoleServiceClient gameConsoleServiceClient;

        /// <summary>
        /// The game console callback
        /// </summary>
        private GameConsoleCallback gameConsoleCallback;

        /// <summary>
        /// Gets or sets the game console service client.
        /// </summary>
        /// <value>
        /// The game console service client.
        /// </value>
        public GameConsoleServiceClient GameConsoleServiceClient
        {
            get
            {
                if (this.gameConsoleServiceClient == null)
                {
                    this.gameConsoleServiceClient = new GameConsoleServiceClient(this.GameConsoleCallback);
                }

                return this.gameConsoleServiceClient;
            }
        }

        /// <summary>
        /// Gets the game console callback.
        /// </summary>
        /// <value>
        /// The game console callback.
        /// </value>
        public GameConsoleCallback GameConsoleCallback
        {
            get
            {
                if (this.gameConsoleCallback == null)
                {
                    this.gameConsoleCallback = new GameConsoleCallback();
                }

                return this.gameConsoleCallback;
            }
        }
    }
}
