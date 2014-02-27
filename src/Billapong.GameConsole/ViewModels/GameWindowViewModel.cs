namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using Animation;
    using Core.Client.UI;
    using Game;
    using Models;
    using Models.Events;
    using Window = Models.Window;

    /// <summary>
    /// Handles the logic of a single window within the game field
    /// </summary>
    public class GameWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The window
        /// </summary>
        private readonly Window window;

        /// <summary>
        /// The ball
        /// </summary>
        private Ball ball;

        /// <summary>
        /// The ball animation task
        /// </summary>
        private BallAnimationTask ballAnimationTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowViewModel"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public GameWindowViewModel(Window window)
        {
            this.window = window;
            this.Holes = new ObservableCollection<Hole>();

            foreach (var hole in this.window.Holes)
            {
                this.Holes.Add(hole);
            }
        }

        /// <summary>
        /// Occurs when the animation within this windows has finished
        /// </summary>
        public event EventHandler AnimationFinished = delegate { };

        /// <summary>
        /// Occurs when the game field is clicked.
        /// </summary>
        public event EventHandler<GameFieldClickedEventArgs> GameFieldClicked = delegate { };

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
        /// Gets the ball.
        /// </summary>
        /// <value>
        /// The ball.
        /// </value>
        public Ball Ball
        {
            get
            {
                return this.ball;
            }

            private set
            {
                this.ball = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ball animation task.
        /// </summary>
        /// <value>
        /// The ball animation task.
        /// </value>
        public BallAnimationTask BallAnimationTask
        {
            get
            {
                return this.ballAnimationTask;
            }

            set
            {
                this.ballAnimationTask = value;
                this.OnPropertyChanged();
            }
        }

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
                return new DelegateCommand<Point>(this.OnGameFieldClicked, this.IsGameFieldClickable);
            }
        }

        /// <summary>
        /// Gets the animation finished command.
        /// </summary>
        /// <value>
        /// The animation finished command.
        /// </value>
        public DelegateCommand AnimationFinishedCommand
        {
            get
            {
                return new DelegateCommand(this.OnAnimationFinished);
            }
        }

        /// <summary>
        /// Places the ball at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public void PlaceBall(Point position, Color color)
        {
            this.Ball = new Ball { Position = position, Color = color };
        }

        /// <summary>
        /// Determines whether the gamefield is clickable
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>The evaluation result</returns>
        private bool IsGameFieldClickable(Point mousePosition)
        {
            return this.Ball != null && GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState == Player.PlayerState.BallPlaced;
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
        /// Called when the animation of the storyboard finished
        /// </summary>
        private void OnAnimationFinished()
        {
            this.Ball = null;
            this.AnimationFinished(this, null);
        }
    }
}
