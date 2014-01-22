namespace Billapong.GameConsole.Configuration
{
    /// <summary>
    /// The game configuration
    /// </summary>
    public static class GameConfiguration
    {
        /// <summary>
        /// The game types
        /// </summary>
        public enum GameType
        {
            /// <summary>
            /// The single player training
            /// </summary>
            SinglePlayerTraining,

            /// <summary>
            /// The single player game
            /// </summary>
            SinglePlayerGame,

            /// <summary>
            /// The multi player game
            /// </summary>
            MultiPlayerGame
        }

        public const int GameWindowWidth = 300;

        public const int GameWindowHeight = 300;

        public const int GameGridSize = 15;

        public const int HoleDiameter = GameWindowWidth / GameGridSize;

        public const int MaxNumberOfHorizontalGameWindows = 4;

        public const int MaxNumberOfVerticalWindows = 3;
    }
}
