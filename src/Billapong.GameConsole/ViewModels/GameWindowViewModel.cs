namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
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
        private readonly Window window;

        public event EventHandler AnimationFinished = delegate { };

        /// <summary>
        /// The ball
        /// </summary>
        private Ball ball;

        private BallAnimationTask ballAnimationTask;

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
                OnPropertyChanged();
            } 
        }

        /// <summary>
        /// Gets the ball animation queue.
        /// </summary>
        /// <value>
        /// The ball animation queue.
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
                OnPropertyChanged();
            } 
        }

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

        public DelegateCommand AnimationFinishedCommand
        {
            get
            {
                return new DelegateCommand(this.OnAnimationFinished);
            }
        }

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
        /// Places the ball at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public void PlaceBall(Point position, Color color)
        {
            this.Ball = new Ball {Position = position, Color = color};
        }

        /// <summary>
        /// Determines whether the gamefield is clickable
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>The evaluation result</returns>
        private bool IsGameFieldClickable(Point mousePosition)
        {
            return this.Ball != null;
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

        private void OnAnimationFinished()
        {
            this.Ball = null;
            this.AnimationFinished(this, null);
        }
    }
}
