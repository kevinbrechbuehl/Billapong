namespace Billapong.GameConsole.Views
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    using Billapong.GameConsole.Animation;

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly ConcurrentQueue<BallAnimationTask> ballAnimationQueue =
            new ConcurrentQueue<BallAnimationTask>();

        private Storyboard ballStoryboard;

        private Path ballPath;

        private readonly List<Ellipse> holes = new List<Ellipse>();

        private const int GridSizeX = 15;

        private const int GridSizeY = 15;

        private const double BaseAnimationDuration = 2;

        private readonly double ballRadius;

        private readonly double holeDiameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            InitializeComponent();

            this.holeDiameter = this.MapCanvas.Width / GridSizeX;
            this.ballRadius = this.holeDiameter * 0.667 / 2;

            this.FillBallAnimationQueue();
            this.InitializeHoles();
            this.InitializeBall();
            this.InitializeStoryboard();
        }

        /// <summary>
        /// Fills the ball animation queue.
        /// </summary>
        private void FillBallAnimationQueue()
        {
            var task = new BallAnimationTask
                       {
                           NewPosition = new Point(this.ballRadius, 150),
                           Duration = TimeSpan.FromSeconds(1)
                       };
            this.ballAnimationQueue.Enqueue(task);

            task = new BallAnimationTask { NewPosition = new Point(300, 200), Duration = TimeSpan.FromSeconds(2) };
            this.ballAnimationQueue.Enqueue(task);

            task = new BallAnimationTask
                   {
                       NewPosition = new Point(40, this.ballRadius),
                       Duration = TimeSpan.FromSeconds(3)
                   };
            this.ballAnimationQueue.Enqueue(task);

            task = new BallAnimationTask
                   {
                       NewPosition = new Point(220, 300 - this.ballRadius),
                       Duration = TimeSpan.FromSeconds(4)
                   };
            this.ballAnimationQueue.Enqueue(task);
        }

        /// <summary>
        /// Initializes the holes.
        /// </summary>
        private void InitializeHoles()
        {
            var random = new Random(DateTime.Now.GetHashCode());

            for (var i = 0; i < 5; i++)
            {
                var hole = new Ellipse()
                           {
                               Fill = new SolidColorBrush(Colors.Black),
                               Height = this.holeDiameter,
                               Width = this.holeDiameter
                           };

                this.holes.Add(hole);
                this.MapCanvas.Children.Add(hole);

                SetLocation(
                    hole,
                    new Point(random.Next(0, GridSizeX - 1) * hole.Width, random.Next(0, GridSizeY - 1) * hole.Height));
            }
        }

        /// <summary>
        /// Initializes the ball.
        /// </summary>
        private void InitializeBall()
        {
            var ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new Point(200, 100);
            ellipseGeometry.RadiusX = this.ballRadius;
            ellipseGeometry.RadiusY = this.ballRadius;

            this.ballPath = new Path { Fill = Brushes.Red, Data = ellipseGeometry };

            NameScope.SetNameScope(this, new NameScope());
            this.RegisterName("BallEllipseGeometry", ellipseGeometry);
            this.MapCanvas.Children.Add(this.ballPath);
        }

        /// <summary>
        /// Initializes the storyboard.
        /// </summary>
        private void InitializeStoryboard()
        {
            this.ballStoryboard = new Storyboard();

            var pointAnimation = new PointAnimation();
            pointAnimation.By = new Point(200, 100);
            pointAnimation.To = new Point(50, this.ballRadius);
            pointAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(pointAnimation, "BallEllipseGeometry");
            Storyboard.SetTargetProperty(pointAnimation, new PropertyPath(EllipseGeometry.CenterProperty));

            this.ballStoryboard.Children.Add(pointAnimation);
            this.ballStoryboard.Completed += this.BallAnimation_Finished;
        }

        /// <summary>
        /// Handles the Finished event of the BallAnimation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BallAnimation_Finished(object sender, EventArgs e)
        {
            if (!this.ballAnimationQueue.IsEmpty)
            {
                BallAnimationTask task;
                if (this.ballAnimationQueue.TryDequeue(out task))
                {
                    var animation = (PointAnimation)this.ballStoryboard.Children.ElementAt(0);
                    animation.By = animation.To;
                    animation.To = task.NewPosition;
                    animation.Duration = task.Duration;
                    this.ballStoryboard.Begin(this);
                }
            }
        }

        /// <summary>
        /// Balls the hit hole.
        /// </summary>
        /// <param name="ball">The ball.</param>
        /// <param name="hole">The hole.</param>
        /// <returns></returns>
        private bool BallHitHole(EllipseGeometry ball, Ellipse hole)
        {
            var centerOfBall = ball.Center;

            var holePosition = GetLocation(hole);
            var centerOfHole = new Point(holePosition.X + (hole.Width / 2), holePosition.Y + (hole.Height / 2));

            var xRadius = hole.Width / 2;
            var yRadius = hole.Height / 2;

            if (xRadius <= 0.0 || yRadius <= 0.0) return false;

            var normalizedPoint = new Point(centerOfHole.X - centerOfBall.X, centerOfHole.Y - centerOfBall.Y);

            return ((normalizedPoint.X * normalizedPoint.X) / (xRadius * xRadius))
                   + ((normalizedPoint.Y * normalizedPoint.Y) / (yRadius * yRadius)) <= 1.0;
        }

        /// <summary>
        /// Handles the OnMouseDown event of the MapCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void MapCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ballStoryboard.Begin(this);
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
        /// Gets the location.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        private static Point GetLocation(UIElement frameworkElement)
        {
            return new Point(Canvas.GetLeft(frameworkElement), Canvas.GetTop(frameworkElement));
        }
    }
}
