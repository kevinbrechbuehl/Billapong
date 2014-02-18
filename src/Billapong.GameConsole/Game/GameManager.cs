namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
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
        /// <summary>
        /// The instance
        /// </summary>
        private static readonly GameManager SingletonInstance = new GameManager();

        /// <summary>
        /// The list with all game window view models
        /// </summary>
        private readonly List<GameWindowViewModel> gameWindowViewModels = new List<GameWindowViewModel>();

        /// <summary>
        /// The game windows
        /// </summary>
        private readonly List<Window> gameWindows = new List<Window>();

        /// <summary>
        /// The game controller
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// The ball animation task queue
        /// </summary>
        private Queue<BallAnimationTask> ballAnimationTaskQueue;

        /// <summary>
        /// Defines the possible intersections between a ball and the game field
        /// </summary>
        private enum Intersection
        {
            /// <summary>
            /// No intersection
            /// </summary>
            None,

            /// <summary>
            /// The ball hits a hole
            /// </summary>
            Hole,

            /// <summary>
            /// The ball hits the top wall
            /// </summary>
            TopWall,

            /// <summary>
            /// The ball hits the left wall
            /// </summary>
            LeftWall,

            /// <summary>
            /// The ball hits the right wall
            /// </summary>
            RightWall,

            /// <summary>
            /// The ball hits the bottom wall
            /// </summary>
            BottomWall
        }

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

        /// <summary>
        /// Gets the current game.
        /// </summary>
        /// <value>
        /// The current game.
        /// </value>
        public Game CurrentGame { get; private set; }

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
            this.gameController.RoundEnded += this.EndRound;
            this.gameController.GameCancelled += this.CancelGame;

            this.OpenGameField();

            if (this.CurrentGame.StartGame)
            {
                this.PlaceBallOnGameField();
            }
        }

        /// <summary>
        /// Cancels the game if it is running.
        /// </summary>
        public void CancelGame()
        {
            if (this.gameController != null)
            {
                this.gameController.CancelGame();
            }
        }

        /// <summary>
        /// Cancels the game based on an event callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CancelGame(object sender, EventArgs args)
        {
            if (this.CurrentGame != null)
            {
                this.CloseGameField();
                this.CurrentGame = null;
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

            var initialVerticalOffset = (SystemParameters.WorkArea.Height / 2) - (gameFieldHeight / 2)
                                        - ((maxWindowRow * WindowBorderOffset) / 2);
            var initialHorizontalOffset = (SystemParameters.WorkArea.Width / 2) - (gameFieldWidth / 2)
                                          - ((maxWindowCol * WindowBorderOffset) / 2);

            var verticalOffset = initialVerticalOffset;

            for (var currentRow = 0; currentRow <= maxWindowRow; currentRow++)
            {
                var horizontalOffset = initialHorizontalOffset;
                var windowsInRow =
                    this.CurrentGame.Map.Windows.Where(window => window.Y == currentRow)
                        .OrderBy(window => window.X)
                        .ToList();

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

                        this.gameWindowViewModels.Add(gameWindowViewModel);
                        this.gameWindows.Add(gameWindow);

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
        /// Closes the game field.
        /// </summary>
        private void CloseGameField()
        {
            foreach (var window in this.gameWindows)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Places the ball on game field.
        /// </summary>
        private void PlaceBallOnGameField()
        {
            var possibleStartWindows = this.gameWindowViewModels.Where(x => x.Window.IsOwnWindow).ToArray();
            var random = new Random(DateTime.Now.GetHashCode());
            var randomWindow = possibleStartWindows.ElementAt(random.Next(0, possibleStartWindows.Count()));

            var positionFound = false;
            var pointX = 0;
            var pointY = 0;

            // todo (mathp2): Possible endless loop if a map has a hole on every position. Performance is also not very good (many repetitions).
            while (!positionFound)
            {
                var x = random.Next(0, GameConfiguration.GameGridSize);
                var y = random.Next(0, GameConfiguration.GameGridSize);

                if (randomWindow.Holes.FirstOrDefault(hole => hole.X == x && hole.Y == y) == null)
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
            var viewModel = this.gameWindowViewModels.FirstOrDefault(x => x.Window.Id == args.WindowId);
            if (viewModel != null)
            {
                const int GridElementSize = GameConfiguration.GameWindowWidth / GameConfiguration.GameGridSize;

                var positionX = (GridElementSize * args.Position.X) +
                                ((GridElementSize - GameConfiguration.BallDiameter) / 2);
                var positionY = (GridElementSize * args.Position.Y) +
                                ((GridElementSize - GameConfiguration.BallDiameter) / 2);
                var position = new Point(positionX, positionY);

                this.CurrentGame.CurrentBallPosition = position;
                this.CurrentGame.CurrentWindow = viewModel.Window;
                viewModel.PlaceBall(position, this.CurrentGame.CurrentPlayer.PlayerColor);
            }
        }

        /// <summary>
        /// Starts the round. Gets called from the RoundStarted event of the game controller
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoundStartedEventArgs"/> instance containing the event data.</param>
        private void StartRound(object sender, RoundStartedEventArgs args)
        {
            this.ballAnimationTaskQueue = this.CalculateBallAnimationTasks(
                this.CurrentGame.CurrentWindow,
                this.CurrentGame.CurrentBallPosition,
                args.Direction);

            if (this.ballAnimationTaskQueue.Count > 0)
            {
                var firstTask = this.ballAnimationTaskQueue.Dequeue();
                var viewModel = this.gameWindowViewModels.FirstOrDefault(x => x.Window.Id == firstTask.Window.Id);
                if (viewModel != null)
                {
                    viewModel.BallAnimationTask = firstTask;
                }
            }
        }

        /// <summary>
        /// Ends the round. Gets called from the RoundEnded event of the game controller
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoundEndedEventArgs"/> instance containing the event data.</param>
        private void EndRound(object sender, RoundEndedEventArgs args)
        {
            if (args.GameEnded)
            {
                MessageBox.Show("This was the last round. Game ended");

                // todo (mathp2): Here we need to end the game for the user
            }
            else
            {
                // Start the next round if the local player is next
                if (this.CurrentGame.CurrentPlayer.IsLocalPlayer)
                {
                    this.PlaceBallOnGameField();
                }
            }
        }

        /// <summary>
        /// Gets called when the game field is clicked. Occurs only if a ball is in the senders window
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameFieldClickedEventArgs"/> instance containing the event data.</param>
        private void GameFieldClicked(object sender, GameFieldClickedEventArgs args)
        {
            var ballPosition = this.CurrentGame.CurrentBallPosition;
            var direction = new Vector(args.MousePosition.X, args.MousePosition.Y) -
                            new Vector(ballPosition.X, ballPosition.Y);

            this.gameController.StartRound(direction);
        }

        /// <summary>
        /// Gets called when an animation of a window finishes. Ends the round if the queue is empty
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AnimationFinished(object sender, EventArgs args)
        {
            if (this.ballAnimationTaskQueue == null || this.ballAnimationTaskQueue.Count == 0)
            {
                // todo (mathp2): Here we need to know the score of the current round
                if (this.CurrentGame.CurrentPlayer.IsLocalPlayer) { 
                    this.gameController.EndRound(this.CurrentGame.CurrentPlayer.IsFirstPlayer, 1000);
                }

                return;
            }

            var nextTask = this.ballAnimationTaskQueue.Dequeue();
            var viewModel = this.gameWindowViewModels.FirstOrDefault(w => w.Window.Id == nextTask.Window.Id);
            if (viewModel == null) return;

            var nextBallStartPosition = nextTask.Steps.First().By;
            if (nextBallStartPosition != null)
            {
                viewModel.PlaceBall(nextBallStartPosition.Value, this.CurrentGame.CurrentPlayer.PlayerColor);
                viewModel.BallAnimationTask = nextTask;
            }
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
            var isLastAnimation = false;
            var currentWindow = initialWindow;
            var currentBallPosition = startPosition;
            var currentDirection = initialDirection;
            var stepCounter = 0;
            var currentTask = new BallAnimationTask { Window = currentWindow };
            var previousIntersection = Intersection.None;
            var windowPreviouslyChanged = false;
            
            while (!isLastAnimation)
            {
                // Ensure an end of the animation
                // todo (mathp2): Probably by cumulated time instead of steps?
                stepCounter++;
                if (stepCounter == 30)
                {
                    isLastAnimation = true;
                    currentTask.IsLastAnimation = true;
                }

                // Calculate the ignorable intersection
                var ignoredIntersection = Intersection.None;
                if (previousIntersection != Intersection.None)
                {
                    if (windowPreviouslyChanged)
                    {
                        switch (previousIntersection)
                        {
                            case Intersection.TopWall:
                                ignoredIntersection = Intersection.BottomWall;
                                break;
                            case Intersection.LeftWall:
                                ignoredIntersection = Intersection.RightWall;
                                break;
                            case Intersection.RightWall:
                                ignoredIntersection = Intersection.LeftWall;
                                break;
                            case Intersection.BottomWall:
                                ignoredIntersection = Intersection.TopWall;
                                break;
                        }
                    }
                    else
                    {
                        ignoredIntersection = previousIntersection;
                    }
                }

                var currentIntersection = Intersection.None;
                Point? intersectionPosition = null;
                Models.Window neighbourWindow = null;

                /* todo (mathp2): We need a better way to get a point outside of the window */
                var intersectionTestPoint = currentBallPosition + (currentDirection * 1000);

                // Check for hole intersection
                if (currentWindow.Holes != null) 
                { 
                    foreach (var hole in currentWindow.Holes)
                    {
                        Point? firstIntersection;
                        Point? secondIntersection;

                        var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                            hole.CenterPosition,
                            hole.Radius,
                            currentBallPosition,
                            intersectionTestPoint,
                            out firstIntersection,
                            out secondIntersection);

                        if (intersection > 0 && firstIntersection != null)
                        {
                            currentIntersection = Intersection.Hole;

                            // This hole intersection is only relevant, if the distance between this intersection and the ball is the shortest
                            if (intersectionPosition == null
                                || currentBallPosition.DistanceTo((Point)firstIntersection)
                                < currentBallPosition.DistanceTo((Point)intersectionPosition))
                            {
                                intersectionPosition = firstIntersection;    
                            }
                        }    
                    }
                }

                // Check for top wall hit
                if (intersectionPosition == null && ignoredIntersection != Intersection.TopWall)
                {
                    intersectionPosition = CalculationHelpers.GetLineIntersection(
                        currentBallPosition,
                        intersectionTestPoint,
                        new Point(0, 0),
                        new Point(GameConfiguration.GameWindowWidth, 0));

                    if (intersectionPosition != null)
                    {
                        previousIntersection = currentIntersection = Intersection.TopWall;

                        var upperWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y - 1);
                        if (upperWindow != null)
                        {
                            neighbourWindow = upperWindow;
                        }
                    }
                }

                if (intersectionPosition == null && ignoredIntersection != Intersection.LeftWall)
                {
                    // Check for left wall hit
                    intersectionPosition = CalculationHelpers.GetLineIntersection(
                        currentBallPosition,
                        intersectionTestPoint,
                        new Point(0, 0),
                        new Point(0, GameConfiguration.GameWindowWidth));

                    if (intersectionPosition != null)
                    {
                        previousIntersection = currentIntersection = Intersection.LeftWall;

                        var leftWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X - 1 && w.Y == currentWindow.Y);
                        if (leftWindow != null)
                        {
                            neighbourWindow = leftWindow;
                        }
                    }
                }

                if (intersectionPosition == null && ignoredIntersection != Intersection.RightWall)
                {
                    // Check for right wall hit
                    intersectionPosition = CalculationHelpers.GetLineIntersection(
                        currentBallPosition,
                        intersectionTestPoint,
                        new Point(GameConfiguration.GameWindowWidth, 0),
                        new Point(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));

                    if (intersectionPosition != null)
                    {
                        previousIntersection = currentIntersection = Intersection.RightWall;

                        var rightWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X + 1 && w.Y == currentWindow.Y);
                        if (rightWindow != null)
                        {
                            neighbourWindow = rightWindow;
                        }
                    }
                }

                if (intersectionPosition == null && ignoredIntersection != Intersection.BottomWall)
                {
                    // Check for bottom wall hit
                    intersectionPosition = CalculationHelpers.GetLineIntersection(
                        currentBallPosition,
                        intersectionTestPoint,
                        new Point(0, GameConfiguration.GameWindowWidth),
                        new Point(GameConfiguration.GameWindowWidth, GameConfiguration.GameWindowWidth));

                    if (intersectionPosition != null)
                    {
                        previousIntersection = currentIntersection = Intersection.BottomWall;

                        var bottomWindow = this.CurrentGame.Map.Windows.FirstOrDefault(w => w.X == currentWindow.X && w.Y == currentWindow.Y + 1);
                        if (bottomWindow != null)
                        {
                            neighbourWindow = bottomWindow;
                        }
                    }
                }

                if (currentIntersection != Intersection.None && intersectionPosition != null)
                {
                    var newPosition = (Point)intersectionPosition;
                    currentTask.Steps.Add(AnimationHelpers.GetPointAnimation(currentBallPosition, newPosition));

                    // If the ball intersects a hole, end the calculation after this step
                    if (currentIntersection == Intersection.Hole)
                    {
                        currentTask.IsLastAnimation = true;
                        animationQueue.Enqueue(currentTask);
                        return animationQueue;
                    }

                    currentBallPosition = newPosition;

                    // Ball will leave the window 
                    if (neighbourWindow != null)
                    {
                        animationQueue.Enqueue(currentTask);
                        currentWindow = neighbourWindow;
                        currentTask = new BallAnimationTask { Window = currentWindow };
                        windowPreviouslyChanged = true;

                        // Set new initial position for the next window
                        switch (currentIntersection)
                        {
                            case Intersection.TopWall:
                                currentBallPosition.Y = GameConfiguration.GameWindowWidth;
                                break;
                            case Intersection.LeftWall:
                                currentBallPosition.X = GameConfiguration.GameWindowWidth;
                                break;
                            case Intersection.RightWall:
                                currentBallPosition.X = 0;
                                break;
                            case Intersection.BottomWall:
                                currentBallPosition.Y = 0;
                                break;
                        }
                    }
                    else
                    {
                        // Change ball direction within the current window
                        if (currentIntersection == Intersection.TopWall || currentIntersection == Intersection.BottomWall)
                        {
                            currentDirection.Y *= -1;
                        }
                        else
                        {
                            currentDirection.X *= -1;
                        }

                        windowPreviouslyChanged = false;
                    }
                }
            }

            // Add the last animation task to the queue because we had no wall hit (timeout)
            if (currentTask.Steps.Count > 0)
            {
                animationQueue.Enqueue(currentTask);
            }

            return animationQueue;
        }
    }
}
