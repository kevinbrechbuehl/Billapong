namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
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
        private enum WallHit
        {
            None,
            Top,
            Left,
            Right,
            Bottom
        }

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
            const int WindowBorderOffset = 2;

            double maxWindowRow = this.CurrentGame.Map.Windows.Max(window => window.Y);
            double maxWindowCol = this.CurrentGame.Map.Windows.Max(window => window.X);

            var gameFieldHeight = (maxWindowRow + 1) * GameConfiguration.GameWindowHeight;
            var gameFieldWidth = (maxWindowCol + 1) * GameConfiguration.GameWindowWidth;

            var initialVerticalOffset = (SystemParameters.WorkArea.Height / 2) - (gameFieldHeight / 2) - ((maxWindowRow * WindowBorderOffset) / 2);
            var initialHorizontalOffset = (SystemParameters.WorkArea.Width / 2) - (gameFieldWidth / 2) - ((maxWindowCol * WindowBorderOffset) / 2);

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
                        gameWindow.MinHeight = GameConfiguration.GameWindowHeight + WindowBorderOffset;
                        gameWindow.MinWidth = GameConfiguration.GameWindowWidth + WindowBorderOffset;
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

                    horizontalOffset += GameConfiguration.GameWindowWidth + WindowBorderOffset;
                }

                verticalOffset += GameConfiguration.GameWindowHeight + WindowBorderOffset;
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
            args = new BallPlacedOnGameFieldEventArgs(29, new Point(7, 7));

            var viewModel = this.windows.FirstOrDefault(x => x.Key.Window.Id == args.WindowId).Key;
            if (viewModel != null)
            {
                var gridElementSize = GameConfiguration.GameWindowWidth / GameConfiguration.GameGridSize;

                var positionX = (gridElementSize * args.Position.X) + ((gridElementSize - GameConfiguration.BallDiameter) / 2);
                var positionY = (gridElementSize * args.Position.Y) + ((gridElementSize - GameConfiguration.BallDiameter) / 2);
                var position = new Point(positionX, positionY);

                this.CurrentGame.CurrentBallPosition = position;
                this.CurrentGame.CurrentWindow = viewModel.Window;
                viewModel.PlaceBall(position);
            }
        }

        /// <summary>
        /// Starts the round. Gets called from the RoundStarted event of the game controller
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoundStartedEventArgs"/> instance containing the event data.</param>
        private void StartRound(object sender, RoundStartedEventArgs args)
        {
            this.ballAnimationTaskQueue = this.CalculateBallAnimationTasks(this.CurrentGame.CurrentWindow, this.CurrentGame.CurrentBallPosition, args.Direction);

            var firstTask = this.ballAnimationTaskQueue.Dequeue();
            var viewModel = this.windows.FirstOrDefault(x => x.Key.Window.Id == firstTask.Window.Id).Key;

            viewModel.BallAnimationTask = firstTask;
        }

        /// <summary>
        /// Gets called when the game field is clicked. Occurs only if a ball is in the senders window
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameFieldClickedEventArgs"/> instance containing the event data.</param>
        private void GameFieldClicked(object sender, GameFieldClickedEventArgs args)
        {
            var ballPosition = this.CurrentGame.CurrentBallPosition;
            var direction = new Vector(args.MousePosition.X, args.MousePosition.Y) - new Vector(ballPosition.X, ballPosition.Y);

            this.gameController.StartRound(direction);
        }

        /// <summary>
        /// Calculates the ball animation tasks.
        /// </summary>
        /// <param name="initialWindow">The initial window.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="initialDirection">The initial direction.</param>
        /// <returns>The calculated animation queue</returns>
        private Queue<BallAnimationTask> CalculateBallAnimationTasks(Models.Window initialWindow, Point startPosition, Vector initialDirection)
        {
            var animationQueue = new Queue<BallAnimationTask>();

            var lastAnimation = false;

            var currentWindow = initialWindow;
            var currentBallPosition = startPosition;
            var currentBallPositionVector = new Vector(currentBallPosition.X, currentBallPosition.Y);
            var currentDirection = initialDirection;
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

                var wallHit = WallHit.None;
                Models.Window neighbourWindow = null;

                currentBallPositionVector = new Vector(currentBallPosition.X, currentBallPosition.Y);

                /* todo (mathp2): We need a better way to get a point outside of the window */
                var intersectionTestPoint = currentBallPosition + (currentDirection * 1000);
                var intersectionTestVector = new Vector(intersectionTestPoint.X, intersectionTestPoint.Y);

                // Check for top wall hit
                var intersectionPoint = this.Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, 0), new Vector(GameConfiguration.GameWindowWidth, 0));
                if (intersectionPoint != null)
                {
                    wallHit = WallHit.Top;

                    var upperWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y - 1);
                    if (upperWindow != null)
                    {
                        neighbourWindow = upperWindow;
                    }
                }

                if (intersectionPoint == null) 
                {
                    // Check for left wall hit
                    intersectionPoint = this.Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, 0), new Vector(0, GameConfiguration.GameWindowWidth));
                    if (intersectionPoint != null)
                    {
                        wallHit = WallHit.Left;

                        var leftWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X - 1 && w.Y == currentWindow.Y);
                        if (leftWindow != null)
                        {
                            neighbourWindow = leftWindow;
                        }
                    }
                }

                if (intersectionPoint == null) 
                { 
                    // Check for right wall hit
                    intersectionPoint = this.Intersects(currentBallPositionVector, intersectionTestVector, new Vector(GameConfiguration.GameWindowWidth, 0), new Vector(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));
                    if (intersectionPoint != null)
                    {
                        wallHit = WallHit.Right;

                        var rightWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X + 1 && w.Y == currentWindow.Y);
                        if (rightWindow != null)
                        {
                            neighbourWindow = rightWindow;
                        }
                    }
                }

                if (intersectionPoint == null)
                {
                    // Check for bottom wall hit
                    intersectionPoint = this.Intersects(currentBallPositionVector, intersectionTestVector, new Vector(0, GameConfiguration.GameWindowWidth), new Vector(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));
                    if (intersectionPoint != null)
                    {
                        wallHit = WallHit.Bottom;

                        var bottomWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y + 1);
                        if (bottomWindow != null)
                        {
                            neighbourWindow = bottomWindow;
                        }
                    }
                }

                if (wallHit != WallHit.None)
                {
                    var newPosition = new Point(intersectionPoint.Value.X, intersectionPoint.Value.Y);
                    currentTask.Steps.Add(this.GetPointAnimation(currentBallPosition, newPosition));
                    currentBallPosition = newPosition;

                    // Ball will leave the window 
                    if (neighbourWindow != null)
                    {
                        currentWindow = neighbourWindow;
                        animationQueue.Enqueue(currentTask);
                        currentTask = new BallAnimationTask { Window = currentWindow };

                        // Set new initial position for the next window
                        switch (wallHit)
                        {
                            case WallHit.Top:
                                currentBallPosition.Y = GameConfiguration.GameWindowWidth;
                                break;
                            case WallHit.Left:
                                currentBallPosition.X = GameConfiguration.GameWindowWidth;
                                break;
                            case WallHit.Right:
                                currentBallPosition.X = 0;
                                break;
                            case WallHit.Bottom:
                                currentBallPosition.Y = 0;
                                break;
                        }
                    }
                    else
                    {
                        // Change ball direction within the current window
                        if (wallHit == WallHit.Top || wallHit == WallHit.Bottom)
                        {
                            currentDirection.Y *= -1;
                        }
                        else
                        {
                            currentDirection.X *= -1;
                        }
                    }
                }
            }

            return animationQueue;
        }

        /// <summary>
        /// Gets the point animation.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <returns>The point animation</returns>
        private PointAnimation GetPointAnimation(Point currentPosition, Point targetPosition)
        {
            var maxDistance = Math.Sqrt(Math.Pow(GameConfiguration.GameWindowWidth, 2) + Math.Pow(GameConfiguration.GameWindowHeight, 2));
            var actualDistance = this.DistanceBetween(currentPosition, targetPosition);

            var animation = new PointAnimation();
            animation.By = currentPosition;
            animation.To = targetPosition;
            animation.Duration = TimeSpan.FromMilliseconds(GameConfiguration.BaseAnimationDuration / maxDistance * actualDistance);
            return animation;
        }

        /// <summary>
        /// Gets called when an animation of a window finishes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AnimationFinished(object sender, EventArgs args)
        {
            if (this.ballAnimationTaskQueue == null || this.ballAnimationTaskQueue.Count <= 0) return;
            
            var nextTask = this.ballAnimationTaskQueue.Dequeue();
            var viewModel = this.windows.FirstOrDefault(w => w.Value.Id == nextTask.Window.Id).Key;
            if (viewModel == null) return;

            viewModel.PlaceBall(nextTask.Steps.First().By.Value);
            viewModel.BallAnimationTask = nextTask;
        }

        /// <summary>
        /// Checks two lines for intersection
        /// </summary>
        /// <param name="firstLineStart">The first line start.</param>
        /// <param name="firstLineEnd">The first line end.</param>
        /// <param name="secondLineStart">The second line start.</param>
        /// <param name="secondLineEnd">The second line end.</param>
        /// <returns>The intersection check result</returns>
        private Vector? Intersects(Vector firstLineStart, Vector firstLineEnd, Vector secondLineStart, Vector secondLineEnd)
        {
            var b = firstLineEnd - firstLineStart;
            var d = secondLineEnd - secondLineStart;
            var delta = (b.X * d.Y) - (b.Y * d.X);

            // check for parallel lines (inifite intersection point)
            if (Math.Abs(delta) < 0) return null;

            var c = secondLineStart - firstLineStart;
            var t = ((c.X * d.Y) - (c.Y * d.X)) / delta;
            if (t < 0 || t > 1)
            {
                return null;
            }

            var u = ((c.X * b.Y) - (c.Y * b.X)) / delta;
            if (u < 0 || u > 1)
            {
                return null;
            }

            return firstLineStart + (t * b);
        }

        private double DistanceBetween(Point point1, Point point2)
        {
            var a = (double)(point2.X - point1.X);
            var b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }
    }
}
