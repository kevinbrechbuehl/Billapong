namespace Billapong.GameConsole.Game
{
    using System.Linq;
    using System.Windows;
    using ViewModels;
    using Views;
    using Game = Models.Game;

    /// <summary>
    /// Handles the running game of Billapong
    /// </summary>
    public class GameManager
    {
        private Game currentGame;

        #region Singleton initialization

        /// <summary>
        /// The instance
        /// </summary>
        private readonly static GameManager SingletonInstance = new GameManager();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static GameManager Instance
        {
            get
            {
                return SingletonInstance;
            }
        }

        #endregion

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void StartGame(Game game)
        {
            this.currentGame = game;
            this.OpenGameField();
        }

        /// <summary>
        /// Opens the game field.
        /// </summary>
        private void OpenGameField()
        {
            var maxWindowRow = this.currentGame.Map.Windows.Where(window => window.IsOwnWindow).Max(window => window.X);
            var maxWindowCol = this.currentGame.Map.Windows.Where(window => window.IsOwnWindow).Max(window => window.Y);

            // Todo (mathp2): Die Höhe und Breite des Spielfeldes muss in das initiale offset des obersten linken Fenster fliessen
            var gameFieldHeight = (maxWindowRow+1)*Configuration.GameConfiguration.GameWindowHeight;
            var gameFieldWidth = (maxWindowCol+1)*Configuration.GameConfiguration.GameWindowWidth;

            var verticalOffset = 100;
            const int windowBorderOffset = 12;

            for (var currentRow = 0; currentRow <= maxWindowRow; currentRow++)
            {
                var horizontalOffset = 100;
                var windowsInRow = this.currentGame.Map.Windows.Where(window => window.X == currentRow).Where(window => window.IsOwnWindow).OrderBy(window => window.Y).ToList();

                for (var currentCol = 0; currentCol <= maxWindowCol; currentCol++)
                {
                    var currentWindow = windowsInRow.FirstOrDefault(window => window.Y == currentCol);
                    if (currentWindow != null)
                    {
                        var gameWindow = new GameWindow();
                        gameWindow.Top = verticalOffset;
                        gameWindow.Left = horizontalOffset;
                        gameWindow.WindowStyle = WindowStyle.None;

                        var gameWindowViewModel = new GameWindowViewModel();
                        gameWindow.DataContext = gameWindowViewModel;

                        gameWindow.Show();
                    }

                    horizontalOffset += Configuration.GameConfiguration.GameWindowWidth + windowBorderOffset;
                }

                verticalOffset += Configuration.GameConfiguration.GameWindowHeight + windowBorderOffset;
            }
        }
    }
}
