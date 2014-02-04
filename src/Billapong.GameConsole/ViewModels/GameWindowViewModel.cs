namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Data;
    using Animation;
    using Core.Client.UI;
    using Models;
    using Models.Events;
    using Window = Models.Window;

    public class GameWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The window
        /// </summary>
        private Window window;

        /// <summary>
        /// The current ball animation task
        /// </summary>
        private BallAnimationTask currentBallAnimationTask;

        /// <summary>
        /// Gets the window.
        /// </summary>
        /// <value>
        /// The window.
        /// </value>
        public Window Window
        {
            get
            {
                return this.window;
            }
        }

        /// <summary>
        /// Gets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public ObservableCollection<Hole> Holes { get; private set; }

        /// <summary>
        /// Gets the balls.
        /// </summary>
        /// <value>
        /// The balls.
        /// </value>
        public ObservableCollection<Ball> Balls { get; private set; }

        /// <summary>
        /// Gets the canvas elements.
        /// </summary>
        /// <value>
        /// The canvas elements.
        /// </value>
        public CompositeCollection CanvasElements { get; private set; }

        /// <summary>
        /// Gets the ball animation queue.
        /// </summary>
        /// <value>
        /// The ball animation queue.
        /// </value>
        public Queue<BallAnimationTask> BallAnimationQueue { get; private set; }

        /// <summary>
        /// Occurs when the game field is clicked.
        /// </summary>
        public event EventHandler<GameFieldClickedEventArgs> GameFieldClicked = delegate { };

        /// <summary>
        /// Gets the game field clicked command.
        /// </summary>
        /// <value>
        /// The game field clicked command.
        /// </value>
        public DelegateCommand<Point> GameFieldClickedCommand
        {
            get
            {
                return new DelegateCommand<Point>(OnGameFieldClicked, IsGameFieldClickable);
            }
        }

        /// <summary>
        /// Gets the ball animation finished command.
        /// </summary>
        /// <value>
        /// The ball animation finished command.
        /// </value>
        public DelegateCommand BallAnimationFinishedCommand
        {
            get
            {
                return new DelegateCommand(BallAnimationFinished);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowViewModel"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public GameWindowViewModel(Window window)
        {
            this.window = window;
            this.BallAnimationQueue = new Queue<BallAnimationTask>();
            this.Holes = new ObservableCollection<Hole>();
            this.Balls = new ObservableCollection<Ball>();
            this.CanvasElements = new CompositeCollection();

            foreach (var hole in this.window.Holes)
            {
                this.Holes.Add(hole);
            }

            this.CanvasElements.Add(new CollectionContainer { Collection = this.Holes });
            this.CanvasElements.Add(new CollectionContainer { Collection = this.Balls });
        }

        /// <summary>
        /// Places the ball at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void PlaceBall(Point position)
        {
            this.Balls.Clear();
            var ball = new Ball {Position = position};
            this.Balls.Add(ball);
        }

        /// <summary>
        /// Determines whether the gamefield is clickable
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>The evaluation result</returns>
        private bool IsGameFieldClickable(Point mousePosition)
        {
            return this.Balls.Count > 0;
        }

        /// <summary>
        /// Called when the game field is clicked.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        private void OnGameFieldClicked(Point mousePosition)
        {
            var eventArgs = new GameFieldClickedEventArgs(mousePosition);
            this.GameFieldClicked(this, eventArgs);   
        }

        /// <summary>
        /// Gets called when the current ball animation is finished
        /// </summary>
        private void BallAnimationFinished()
        {
            if (this.BallAnimationQueue.Count > 0)
            {
                var nextTask = this.BallAnimationQueue.Dequeue();
                var ball = new Ball
                {
                    Position = new Point(currentBallAnimationTask.NewPosition.X, currentBallAnimationTask.NewPosition.Y),
                    PointAnimation = nextTask
                };

                currentBallAnimationTask = nextTask;

                this.Balls.Clear();
                this.Balls.Add(ball);
            }
            else
            {
                if (!currentBallAnimationTask.IsLastAnimation) 
                { 
                    this.Balls.Clear();
                }
            }
        }
    }
}
