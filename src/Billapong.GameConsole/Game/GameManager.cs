namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Animation;
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
        public Game CurrentGame { get; private set; }

        public IGameController gameController;

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
        public static GameManager Current
        {
            get
            {
                return SingletonInstance;
            }
        }

        #endregion

        private Queue<BallAnimationTask> ballAnimationTaskQueue; 

        private readonly Dictionary<GameWindowViewModel, Models.Window> windows = new Dictionary<GameWindowViewModel, Models.Window>(); 

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void StartGame(Game game)
        {
            this.CurrentGame = game;
            switch (this.CurrentGame.GameType)
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

            this.gameController.BallPlacedOnGameField += this.PlaceBallOnGameField;
            this.gameController.RoundStarted += this.StartRound;

            this.OpenGameField();

            if (this.CurrentGame.StartGame)
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

            double maxWindowRow = this.CurrentGame.Map.Windows.Max(window => window.Y);
            double maxWindowCol = this.CurrentGame.Map.Windows.Max(window => window.X);

            var gameFieldHeight = (maxWindowRow+1) * GameConfiguration.GameWindowHeight;
            var gameFieldWidth = (maxWindowCol+1) * GameConfiguration.GameWindowWidth;

            var initialVerticalOffset = (SystemParameters.WorkArea.Height / 2) - (gameFieldHeight / 2) - ((maxWindowRow * windowBorderOffset) / 2);
            var initialHorizontalOffset = (SystemParameters.WorkArea.Width / 2) - (gameFieldWidth / 2) - ((maxWindowCol * windowBorderOffset) / 2);

            var verticalOffset = initialVerticalOffset;

            for (var currentRow = 0; currentRow <= maxWindowRow; currentRow++)
            {
                var horizontalOffset = initialHorizontalOffset;
                var windowsInRow = this.CurrentGame.Map.Windows.Where(window => window.Y == currentRow).OrderBy(window => window.X).ToList();

                for (var currentCol = 0; currentCol <= maxWindowCol; currentCol++)
                {
                    var currentWindow = windowsInRow.FirstOrDefault(window => window.X == currentCol);
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
                        gameWindow.BorderBrush = new SolidColorBrush(this.CurrentGame.StartGame ? Colors.Red : Colors.Blue);
                        gameWindow.BorderThickness = new Thickness(1, 1, 1, 1);

                        var gameWindowViewModel = new GameWindowViewModel(currentWindow);
                        gameWindow.DataContext = gameWindowViewModel;
                        gameWindowViewModel.GameFieldClicked += this.GameFieldClicked;
                        gameWindowViewModel.AnimationFinished += this.AnimationFinished;

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

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
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

            this.gameController.PlaceBallOnGameField(randomWindow.Window.Id, new Point(pointX, pointY));
        }

        /// <summary>
        /// Places the ball on game field based on an event callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="BallPlacedOnGameFieldEventArgs"/> instance containing the event data.</param>
        private void PlaceBallOnGameField(object sender, BallPlacedOnGameFieldEventArgs args)
        {
            var viewModel = this.windows.FirstOrDefault(x => x.Key.Window.Id == args.WindowId).Key;
            if (viewModel != null)
            {
                var position = new Point(args.Position.X * (GameConfiguration.BallRadius * 2) - GameConfiguration.BallRadius,
                    args.Position.Y * (GameConfiguration.BallRadius * 2) - GameConfiguration.BallRadius);
                this.CurrentGame.CurrentBallPosition = position;
                this.CurrentGame.CurrentWindow = viewModel.Window;
                viewModel.PlaceBall(position);
            }
        }

        private void StartRound(object sender, RoundStartedEventArgs args)
        {
            
        }

        private void GameFieldClicked(object sender, GameFieldClickedEventArgs args)
        {
            var ballPosition = this.CurrentGame.CurrentBallPosition;
            this.ballAnimationTaskQueue = this.CalculateBallAnimationTasks(this.CurrentGame.CurrentWindow, ballPosition, args.Position);

            var firstTask = this.ballAnimationTaskQueue.Dequeue();
            var viewModel = this.windows.FirstOrDefault(x => x.Key.Window.Id == firstTask.Window.Id).Key;

            viewModel.BallAnimationTask = firstTask;
            //this.gameController.StartRound(new Point());
        }

        private Queue<BallAnimationTask> CalculateBallAnimationTasks(Models.Window initialWindow, Point initialBallPosition, Point mousePosition)
        {
            var animationQueue = new Queue<BallAnimationTask>();

            var lastAnimation = false;

            Models.Window currentWindow = initialWindow;
            Point currentBallPosition = initialBallPosition;
            var currentBallPositionVector = new Vector(currentBallPosition.X, currentBallPosition.Y);
            var currentDirection = new Vector(mousePosition.X, mousePosition.Y) - currentBallPositionVector;
            currentDirection.Normalize();
            var counter = 0;

            var currentTask = new BallAnimationTask();
            currentTask.Window = currentWindow;

            while (!lastAnimation)
            {
                counter++;
                if (counter == 30)
                {
                    lastAnimation = true;
                    currentTask.IsLastAnimation = true;
                }

                /* todo (mathp2): We need a better way to get a point outside of the window */
                var intersectionTestPoint = currentBallPosition + (currentDirection * 100000);
                var intersectionTestVector = new Vector(intersectionTestPoint.X, intersectionTestPoint.Y);

                // Check for top wall hit
                var intersectionPoint = Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, 0), new Vector(GameConfiguration.GameWindowWidth, 0));
                if (intersectionPoint != null)
                {
                    currentTask.Steps.Add(GetPointAnimation(currentBallPosition, new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y)));
                    currentBallPosition = new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y);

                    var upperWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y - 1);
                    if (upperWindow == null)
                    {
                        currentDirection.Y *= -1;
                    }
                    else
                    {
                        currentWindow = upperWindow;
                        animationQueue.Enqueue(currentTask);
                        currentTask = new BallAnimationTask();
                        currentTask.Window = currentWindow;
                        currentBallPosition.Y = GameConfiguration.GameWindowHeight;
                    }

                    continue;
                }

                // Check for left wall hit
                intersectionPoint = Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, 0), new Vector(0, GameConfiguration.GameWindowWidth));
                if (intersectionPoint != null)
                {
                    currentTask.Steps.Add(GetPointAnimation(currentBallPosition, new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y)));
                    currentBallPosition = new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y);

                    var leftWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X - 1 && w.Y == currentWindow.Y);
                    if (leftWindow == null)
                    {
                        currentDirection.X *= -1;
                    }
                    else
                    {
                        currentWindow = leftWindow;
                        animationQueue.Enqueue(currentTask);
                        currentTask = new BallAnimationTask();
                        currentTask.Window = currentWindow;
                        currentBallPosition.X = GameConfiguration.GameWindowWidth;
                    }

                    continue;
                }

                // Check for right wall hit
                intersectionPoint = Intersects(currentBallPositionVector, intersectionTestVector, new Vector(GameConfiguration.GameWindowWidth, 0), new Vector(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));
                if (intersectionPoint != null)
                {
                    currentTask.Steps.Add(GetPointAnimation(currentBallPosition, new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y)));
                    currentBallPosition = new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y);

                    var rightWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X + 1 && w.Y == currentWindow.Y);
                    if (rightWindow == null)
                    {
                        currentDirection.X *= -1;
                    }
                    else
                    {
                        currentWindow = rightWindow;
                        animationQueue.Enqueue(currentTask);
                        currentTask = new BallAnimationTask();
                        currentTask.Window = currentWindow;
                        currentBallPosition.X = 0;
                    }

                    continue;
                }

                // Check for bottom wall hit
                intersectionPoint = Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, GameConfiguration.GameWindowWidth), new Vector(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));
                if (intersectionPoint != null)
                {
                    currentTask.Steps.Add(GetPointAnimation(currentBallPosition, new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y)));
                    currentBallPosition = new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y);

                    var bottomWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y + 1);
                    if (bottomWindow == null)
                    {
                        currentDirection.Y *= -1;
                    }
                    else
                    {
                        currentWindow = bottomWindow;
                        animationQueue.Enqueue(currentTask);
                        currentTask = new BallAnimationTask();
                        currentTask.Window = currentWindow;
                        currentBallPosition.Y = 0;
                    }
                }
            }

            // Add the last animation
            animationQueue.Enqueue(currentTask);

            return animationQueue;
        }

        private PointAnimation GetPointAnimation(Point currentPosition, Point newPosition)
        {
            var animation = new PointAnimation();
            animation.By = currentPosition;
            animation.To = newPosition;
            animation.Duration = TimeSpan.FromSeconds(1);
            return animation;
        }

        private void AnimationFinished(object sender, EventArgs args)
        {
            if (this.ballAnimationTaskQueue != null && this.ballAnimationTaskQueue.Count > 0)
            {
                var nextTask = this.ballAnimationTaskQueue.Dequeue();
                var viewModel = this.windows.FirstOrDefault(w => w.Value.Id == nextTask.Window.Id).Key;
                if (viewModel != null)
                {
                    viewModel.PlaceBall(nextTask.Steps.First().By.Value);
                    viewModel.BallAnimationTask = nextTask;
                }
            }
        }

        /// <summary>
        /// Checks two lines for intersection
        /// </summary>
        /// <param name="firstLineStart">The first line start.</param>
        /// <param name="firstLineEnd">The first line end.</param>
        /// <param name="secondLineStart">The second line start.</param>
        /// <param name="secondLineEnd">The second line end.</param>
        /// <returns></returns>
        public Vector? Intersects(Vector firstLineStart, Vector firstLineEnd, Vector secondLineStart, Vector secondLineEnd)
        {
            Vector b = firstLineEnd - firstLineStart;
            Vector d = secondLineEnd - secondLineStart;
            var bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return null;

            Vector c = secondLineStart - firstLineStart;
            var t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
            {
                return null;
            }

            var u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
            {
                return null;
            }

            return firstLineStart + t * b;
        }
    }
}
