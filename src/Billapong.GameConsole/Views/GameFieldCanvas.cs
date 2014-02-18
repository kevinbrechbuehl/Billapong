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
    using Configuration;
    using Models;

    public class GameFieldCanvas : Canvas
    {
        public GameFieldCanvas()
        {
            this.Width = GameConfiguration.GameWindowWidth;
            this.Height = GameConfiguration.GameWindowHeight;
        }

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof (ICommand), typeof(GameFieldCanvas), new PropertyMetadata(ClickCommandChanged));

        public static readonly DependencyProperty AnimationFinishedProperty = DependencyProperty.Register("AnimationFinished", typeof (ICommand), typeof(GameFieldCanvas), new PropertyMetadata(AnimationFinishedChanged));

        public static readonly DependencyProperty HolesProperty = DependencyProperty.Register("Holes", typeof(IEnumerable<Hole>), typeof(GameFieldCanvas), new PropertyMetadata(HolesChanged));

        public static readonly DependencyProperty BallProperty = DependencyProperty.Register("Ball", typeof(Ball), typeof(GameFieldCanvas), new PropertyMetadata(BallChanged));

        public static readonly DependencyProperty BallAnimationTaskProperty = DependencyProperty.Register("BallAnimationTask", typeof(BallAnimationTask), typeof(GameFieldCanvas), new PropertyMetadata(BallAnimationTaskChanged));

        private Path ballPath;

        private Storyboard ballStoryboard;

        public ICommand AnimationFinished
        {
            get { return (ICommand) GetValue(AnimationFinishedProperty); }
            set { SetValue(AnimationFinishedProperty, value); }
        }

        public static void AnimationFinishedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        public BallAnimationTask BallAnimationTask
        {
            get { return (BallAnimationTask)GetValue(BallAnimationTaskProperty); }
            set { SetValue(BallAnimationTaskProperty, value); }
        }

        public Ball Ball
        {
            get { return (Ball) GetValue(BallProperty); }
            set { SetValue(BallProperty, value); }
        }

        public IEnumerable<Hole> Holes
        {
            get { return (IEnumerable<Hole>) GetValue(HolesProperty); }
            set { SetValue(HolesProperty, value); }
        }

        public ICommand ClickCommand
        {
            get { return (ICommand) GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static void HolesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas) obj;
            canvas.SetHoles();
        }

        public static void BallChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;
            canvas.HandleBall();
        }

        public static void BallAnimationTaskChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;
            canvas.AnimateBall();
        }

        public static void ClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void SetHoles()
        {
            foreach (var hole in this.Holes)
            {
                var holeEllipse = new Ellipse();
                holeEllipse.Width = hole.Diameter;
                holeEllipse.Height = hole.Diameter;
                holeEllipse.Fill = new SolidColorBrush(Colors.Black);
                this.Children.Add(holeEllipse);

                SetLocation(holeEllipse, new Point(hole.Left, hole.Top));
            }
        }

        private void HandleBall()
        {
            if (this.Ball != null && this.ballPath == null)
            {
                var ellipseGeometry = new EllipseGeometry();
                ellipseGeometry.Center = new Point(this.Ball.Position.X, this.Ball.Position.Y);
                ellipseGeometry.RadiusX = this.Ball.Radius;
                ellipseGeometry.RadiusY = this.Ball.Radius;

                this.ballPath = new Path { Fill = new SolidColorBrush(this.Ball.Color), Data = ellipseGeometry };

                NameScope.SetNameScope(this, new NameScope());
                this.RegisterName("BallEllipseGeometry", ellipseGeometry);
                this.Children.Add(this.ballPath);
            }
            else if (this.Ball == null && this.ballPath != null)
            {
                this.Children.Remove(this.ballPath);
                this.ballPath = null;
            }
        }

        private void AnimateBall()
        {
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

                this.ballStoryboard.Completed += this.BallAnimation_Finished;
                
                this.ballStoryboard.Begin(this);
            }
        }

        private void BallAnimation_Finished(object sender, EventArgs e)
        {
            this.AnimationFinished.Execute(null);
        }

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
            SetLeft(child, point.X);
            SetTop(child, point.Y);
        }

    }
}
