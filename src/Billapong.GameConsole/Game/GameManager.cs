namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Configuration;
    using Models.Events;
    using ViewModels;
    using Views;
    using Game = Models.Game;

    /// <summary>
    /// Handles the running game of Billapong
    /// </summary>
    public class GameManager
    {
        private Game currentGame;

        public Game CurrentGame
        {
            get
            {
                return this.currentGame;
            }
        }

        private IGameController gameController;

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

        private readonly Dictionary<GameWindowViewModel, Models.Window> windows = new Dictionary<GameWindowViewModel, Models.Window>(); 

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void StartGame(Game game)
        {
            this.currentGame = game;
            switch (this.currentGame.GameType)
            {
                case GameConfiguration.GameType.SinglePlayerTraining:
                    this.gameController = new SinglePlayerTrainingGameController();
                    break;
                case GameConfiguration.GameType.MultiPlayerGame:
                    this.gameController = new MultiplayerGameController();
                    break;
                default:
                    this.gameController = new SinglePlayerGameController();
                    break;
            }

            this.OpenGameField();

            if (this.currentGame.StartGame)
            {
                this.PlaceBallOnGameField();
            }
        }

        /// <summary>
        /// Opens the game field.
        /// </summary>
        private void OpenGameField()
        {
            const int windowBorderOffset = 16;

            double maxWindowRow = this.currentGame.Map.Windows.Max(window => window.X);
            double maxWindowCol = this.currentGame.Map.Windows.Max(window => window.Y);

            var gameFieldHeight = (maxWindowRow+1) * GameConfiguration.GameWindowHeight;
            var gameFieldWidth = (maxWindowCol+1) * GameConfiguration.GameWindowWidth;

            var initialVerticalOffset = (SystemParameters.WorkArea.Height/2) - (gameFieldHeight/2) - ((maxWindowRow * windowBorderOffset) / 2);
            var initialHorizontalOffset = (SystemParameters.WorkArea.Width / 2) - (gameFieldWidth / 2) - ((maxWindowCol * windowBorderOffset) / 2);

            var verticalOffset = initialVerticalOffset;

            for (var currentRow = 0; currentRow <= maxWindowRow; currentRow++)
            {
                var horizontalOffset = initialHorizontalOffset;
                var windowsInRow = this.currentGame.Map.Windows.Where(window => window.X == currentRow).OrderBy(window => window.Y).ToList();

                for (var currentCol = 0; currentCol <= maxWindowCol; currentCol++)
                {
                    var currentWindow = windowsInRow.FirstOrDefault(window => window.Y == currentCol);
                    if (currentWindow != null)
                    {
                        var gameWindow = new GameWindow();
                        gameWindow.Top = verticalOffset;
                        gameWindow.Left = horizontalOffset;
                        gameWindow.MinHeight = GameConfiguration.GameWindowHeight + windowBorderOffset;
                        gameWindow.MinWidth = GameConfiguration.GameWindowWidth + windowBorderOffset;
                        gameWindow.MaxHeight = gameWindow.MinHeight;
                        gameWindow.MaxWidth = gameWindow.MinWidth;
                        gameWindow.ResizeMode = ResizeMode.NoResize;
                        gameWindow.WindowStyle = WindowStyle.None;
                        
                        // Todo (mathp2): Remove client color separation somewhen in the future
                        gameWindow.BorderBrush = new SolidColorBrush(this.currentGame.StartGame ? Colors.Red : Colors.Blue);
                        gameWindow.BorderThickness = new Thickness(1, 1, 1, 1);

                        var gameWindowViewModel = new GameWindowViewModel(currentWindow);
                        gameWindow.DataContext = gameWindowViewModel;
                        gameWindowViewModel.GameFieldClicked += this.GameFieldClicked;

                        this.windows.Add(gameWindowViewModel, currentWindow);

                        if (currentWindow.IsOwnWindow)
                        {
                            gameWindow.Show();
                        }
                    }

                    horizontalOffset += GameConfiguration.GameWindowWidth + windowBorderOffset;
                }

                verticalOffset += GameConfiguration.GameWindowHeight + windowBorderOffset;
            }
        }

        private void PlaceBallOnGameField()
        {
            var possibleStartWindows = this.windows.Where(x => x.Value.IsOwnWindow).ToArray();
            var random = new Random(DateTime.Now.GetHashCode());
            var randomWindow = possibleStartWindows.ElementAt(random.Next(0, possibleStartWindows.Count())).Key;

            var positionFound = false;
            var pointX = 0;
            var pointY = 0;
            while (!positionFound)
            {
                int x = random.Next(0, GameConfiguration.GameGridSize);
                int y = random.Next(0, GameConfiguration.GameGridSize);

                if (randomWindow.Window.Holes.FirstOrDefault(hole => hole.X == x && hole.Y == y) == null)
                {
                    pointX = x;
                    pointY = y;
                    positionFound = true;
                }
            }

            this.gameController.BallPlacedOnGameField += this.PlaceBallOnGameField;
            this.gameController.PlaceBallOnGameField(randomWindow.Window.Id, pointX, pointY);
        }

        private void PlaceBallOnGameField(object sender, BallPlacedOnGameFieldEventArgs args)
        {
            var viewModel = this.windows.FirstOrDefault(x => x.Key.Window.Id == args.WindowId).Key;
            if (viewModel != null)
            {
                var position = new Point(args.PointX*(GameConfiguration.BallRadius*2) - GameConfiguration.BallRadius,
                    args.PointY*(GameConfiguration.BallRadius*2) - GameConfiguration.BallRadius);
                viewModel.PlaceBall(position);
                this.gameController.BallPlacedOnGameField -= this.PlaceBallOnGameField;
            }
        }

        private void GameFieldClicked(object sender, GameFieldClickedEventArgs args)
        {
    
        }
    }
}
