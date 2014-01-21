namespace Billapong.GameConsole.Game
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
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
            const int windowBorderOffset = 16;

            double maxWindowRow = this.currentGame.Map.Windows.Max(window => window.X);
            double maxWindowCol = this.currentGame.Map.Windows.Max(window => window.Y);

            var gameFieldHeight = (maxWindowRow+1)*Configuration.GameConfiguration.GameWindowHeight;
            var gameFieldWidth = (maxWindowCol+1)*Configuration.GameConfiguration.GameWindowWidth;

            var initialVerticalOffset = (SystemParameters.PrimaryScreenHeight/2) - (gameFieldHeight/2) - ((maxWindowRow * windowBorderOffset) / 2);
            var initialHorizontalOffset = (SystemParameters.PrimaryScreenWidth / 2) - (gameFieldWidth / 2) - ((maxWindowCol * windowBorderOffset) / 2);

            var verticalOffset = initialVerticalOffset;

            for (var currentRow = 0; currentRow <= maxWindowRow; currentRow++)
            {
                var horizontalOffset = initialHorizontalOffset;
                var windowsInRow = this.currentGame.Map.Windows.Where(window => window.X == currentRow).OrderBy(window => window.Y).ToList();

                for (var currentCol = 0; currentCol <= maxWindowCol; currentCol++)
                {
                    var currentWindow = windowsInRow.FirstOrDefault(window => window.Y == currentCol && window.IsOwnWindow);
                    if (currentWindow != null)
                    {
                        var gameWindow = new GameWindow();
                        gameWindow.Top = verticalOffset;
                        gameWindow.Left = horizontalOffset;
                        
                        // Todo (mathp2): Remove client color separation somewhen in the future
                        gameWindow.WindowStyle = WindowStyle.None;
                        gameWindow.BorderBrush = new SolidColorBrush(this.currentGame.StartGame ? Colors.Red : Colors.Blue);
                        gameWindow.BorderThickness = new Thickness(2, 2, 2, 2);

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
