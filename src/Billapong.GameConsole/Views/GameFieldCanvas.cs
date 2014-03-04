namespace Billapong.GameConsole.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Animation;

    using Billapong.GameConsole.Game;

    using Configuration;
    using Models;

    /// <summary>
    /// Represents a part of the game field
    /// </summary>
    public class GameFieldCanvas : Canvas
    {
        /// <summary>
        /// The click command property
        /// </summary>
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(GameFieldCanvas), new PropertyMetadata(ClickCommandChanged));

        /// <summary>
        /// The animation finished command property
        /// </summary>
        public static readonly DependencyProperty AnimationFinishedCommandProperty = DependencyProperty.Register("AnimationFinishedCommand", typeof(ICommand), typeof(GameFieldCanvas), new PropertyMetadata(AnimationFinishedCommandChanged));

        /// <summary>
        /// The holes property
        /// </summary>
        public static readonly DependencyProperty HolesProperty = DependencyProperty.Register("Holes", typeof(IEnumerable<Hole>), typeof(GameFieldCanvas), new PropertyMetadata(HolesChanged));

        /// <summary>
        /// The ball property
        /// </summary>
        public static readonly DependencyProperty BallProperty = DependencyProperty.Register("Ball", typeof(Ball), typeof(GameFieldCanvas), new PropertyMetadata(BallChanged));

        /// <summary>
        /// The ball animation task property
        /// </summary>
        public static readonly DependencyProperty BallAnimationTaskProperty = DependencyProperty.Register("BallAnimationTask", typeof(BallAnimationTask), typeof(GameFieldCanvas), new PropertyMetadata(BallAnimationTaskChanged));

        /// <summary>
        /// The ball path
        /// </summary>
        private Path ballPath;

        /// <summary>
        /// The queue line
        /// </summary>
        private Line queueLine;

        /// <summary>
        /// The ball storyboard
        /// </summary>
        private Storyboard ballStoryboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFieldCanvas"/> class.
        /// </summary>
        public GameFieldCanvas()
        {
            this.Width = GameConfiguration.GameWindowWidth;
            this.Height = GameConfiguration.GameWindowHeight;
            this.InitializeQueueLine();
        }

        /// <summary>
        /// Gets or sets the animation finished command.
        /// </summary>
        /// <value>
        /// The animation finished.
        /// </value>
        public ICommand AnimationFinishedCommand
        {
            get { return (ICommand)GetValue(AnimationFinishedCommandProperty); }
            set { this.SetValue(AnimationFinishedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ball animation task.
        /// </summary>
        /// <value>
        /// The ball animation task.
        /// </value>
        public BallAnimationTask BallAnimationTask
        {
            get { return (BallAnimationTask)GetValue(BallAnimationTaskProperty); }
            set { this.SetValue(BallAnimationTaskProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ball.
        /// </summary>
        /// <value>
        /// The ball.
        /// </value>
        public Ball Ball
        {
            get { return (Ball)GetValue(BallProperty); }
            set { this.SetValue(BallProperty, value); }
        }

        /// <summary>
        /// Gets or sets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public IEnumerable<Hole> Holes
        {
            get { return (IEnumerable<Hole>)GetValue(HolesProperty); }
            set { this.SetValue(HolesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the click command.
        /// </summary>
        /// <value>
        /// The click command.
        /// </value>
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { this.SetValue(ClickCommandProperty, value); }
        }

        /// <summary>
        /// Gets called, when the animation finished command changed
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void AnimationFinishedCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Gets called when the holes are added or removed
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void HolesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;
            canvas.SetHoles();
        }

        /// <summary>
        /// Gets called when the ball is added or removed
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void BallChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;
            canvas.HandleBall();
        }

        /// <summary>
        /// Gets called when the ball animation task changed
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void BallAnimationTaskChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;
            canvas.AnimateBall();
        }

        /// <summary>
        /// Gets called when the click command changed
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void ClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Gets called when the left mouse button goes up
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (GameFieldCanvas)sender;
            var mousePosition = e.GetPosition((IInputElement)sender);

            if (canvas.ClickCommand.CanExecute(mousePosition))
            {
                canvas.ClickCommand.Execute(mousePosition);
            }
        }

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="point">The point.</param>
        private static void SetLocation(UIElement child, Point point)
        {
            Canvas.SetLeft(child, point.X);
            Canvas.SetTop(child, point.Y);
        }

        /// <summary>
        /// Adds the holes to the game field.
        /// </summary>
        private void SetHoles()
        {
            foreach (var hole in this.Holes)
            {
                var holeEllipse = new Ellipse
                                      {
                                          Width = hole.Diameter,
                                          Height = hole.Diameter,
                                          Fill = new SolidColorBrush(Colors.Black)
                                      };

                this.Children.Add(holeEllipse);
                Panel.SetZIndex(holeEllipse, 0);
                SetLocation(holeEllipse, new Point(hole.Left, hole.Top));
            }
        }

        /// <summary>
        /// Handles the ball.
        /// </summary>
        private void HandleBall()
        {
            if (this.Ball != null && this.ballPath == null)
            {
                this.AddBall();

                if (GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState == Player.PlayerState.BallPlaced) 
                {
                    this.ShowQueueLine();
                }
            }
            else if (this.Ball == null && this.ballPath != null)
            {
                this.RemoveBall();
            }
        }

        /// <summary>
        /// Adds the ball to the current field part.
        /// </summary>
        private void AddBall()
        {
            var ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new Point(this.Ball.Position.X, this.Ball.Position.Y);
            ellipseGeometry.RadiusX = this.Ball.Radius;
            ellipseGeometry.RadiusY = this.Ball.Radius;

            this.ballPath = new Path { Fill = new SolidColorBrush(this.Ball.Color), Data = ellipseGeometry };

            NameScope.SetNameScope(this, new NameScope());
            this.RegisterName("BallEllipseGeometry", ellipseGeometry);
            this.Children.Add(this.ballPath);
            Panel.SetZIndex(this.ballPath, 2);
        }

        /// <summary>
        /// Removes the ball.
        /// </summary>
        private void RemoveBall()
        {
            this.Children.Remove(this.ballPath);
            this.ballPath = null;
        }

        /// <summary>
        /// Animates the ball.
        /// </summary>
        private void AnimateBall()
        {
            this.HideQueueLine();

            if (this.BallAnimationTask != null && this.BallAnimationTask.Steps.Any())
            {
                this.ballStoryboard = new Storyboard();

                var stepBeginTime = TimeSpan.Zero;
               
                foreach (var step in this.BallAnimationTask.Steps)
                {
                    step.BeginTime = stepBeginTime;
                    stepBeginTime += step.Duration.TimeSpan;
                    Storyboard.SetTargetName(step, "BallEllipseGeometry");
                    Storyboard.SetTargetProperty(step, new PropertyPath(EllipseGeometry.CenterProperty));
                    this.ballStoryboard.Children.Add(step);
                }

                this.ballStoryboard.Completed += this.BallAnimationFinished;
                this.ballStoryboard.Begin(this);
            }
        }

        /// <summary>
        /// Initializes the queue line.
        /// </summary>
        private void InitializeQueueLine()
        {
            this.queueLine = new Line
                           {
                               Stroke = new SolidColorBrush(Colors.SlateGray),
                               StrokeThickness = 2,
                               Visibility = Visibility.Collapsed
                           };

            this.Children.Add(this.queueLine);
            Panel.SetZIndex(this.queueLine, 1);
        }

        /// <summary>
        /// Shows the queue line.
        /// </summary>
        private void ShowQueueLine()
        {
            var mousePosition = Mouse.GetPosition(this);
            this.SetQueueLinePosition(mousePosition);
            this.queueLine.Visibility = Visibility.Visible;
            this.MouseMove += this.SetQueueLinePosition;
        }

        /// <summary>
        /// Hides the queue line.
        /// </summary>
        private void HideQueueLine()
        {
            this.queueLine.Visibility = Visibility.Collapsed;
            this.MouseMove -= this.SetQueueLinePosition;
        }

        /// <summary>
        /// Sets the queue line position.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="mouseEventArgs">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void SetQueueLinePosition(object sender, MouseEventArgs mouseEventArgs)
        {
            var mousePosition = mouseEventArgs.GetPosition(this);
            this.SetQueueLinePosition(mousePosition);
        }

        /// <summary>
        /// Sets the queue line position based on the current mouse position.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        private void SetQueueLinePosition(Point mousePosition)
        {
            if (this.Ball != null)
            {
                var horizontalLength = mousePosition.X - this.Ball.Position.X;
                var verticalLength = mousePosition.Y - this.Ball.Position.Y;

                var strokeLength = Math.Sqrt(Math.Pow(horizontalLength, 2) + Math.Pow(verticalLength, 2));
                var lengthNormalizator = Math.Abs(1 / strokeLength * GameConfiguration.QueueLineLength);

                this.queueLine.X1 = this.Ball.Position.X;
                this.queueLine.X2 = this.Ball.Position.X + (horizontalLength * lengthNormalizator);
                this.queueLine.Y1 = this.Ball.Position.Y;
                this.queueLine.Y2 = this.Ball.Position.Y + (verticalLength * lengthNormalizator);
            }
        }

        /// <summary>
        /// Balls the animation finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BallAnimationFinished(object sender, EventArgs e)
        {
            this.AnimationFinishedCommand.Execute(null);
        }
    }
}
