using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Billapong.GameConsole
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly Ellipse ballEllipse;
        private readonly List<Ellipse> holes = new List<Ellipse>();
        private Vector direction;
        private int gridSizeX = 10;
        private int gridSizeY = 10;
        private bool ballMoving;
        private double speed = 10;
        private int wallHits;
        private readonly double ballDiameter;

        public GameWindow()
        {
            InitializeComponent();

            double holeDiameter = mapCanvas.Width/gridSizeX;
            ballDiameter = holeDiameter/2;
            speed = mapCanvas.Width/30;

            var random = new Random();

            // Test holes
            for (var i = 0; i < 5; i++)
            {
                var hole = new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Black),
                    Height = holeDiameter,
                    Width = holeDiameter
                };

                mapCanvas.Children.Add(hole);
                Canvas.SetLeft(hole, random.Next(0, Convert.ToInt32(mapCanvas.Height)));
                Canvas.SetTop(hole, random.Next(0, Convert.ToInt32(mapCanvas.Height)));

                holes.Add(hole);
            }

            // Add ball to map
            ballEllipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                Height = ballDiameter,
                Width = ballDiameter
            };
            mapCanvas.Children.Add(ballEllipse);

            Canvas.SetLeft(ballEllipse, 150);
            Canvas.SetTop(ballEllipse, 90);
        }

        private void Render(object sender, EventArgs e)
        {
            if (ballMoving)
            {
                Point currentPosition = GetLocation(ballEllipse);
                Point newPosition = currentPosition + (direction*speed);

                // Fix border positions if out of bounds
                if (newPosition.X < 0)
                {
                    newPosition.X = 0;
                }
                else if (newPosition.X > mapCanvas.ActualWidth - ballDiameter)
                {
                    newPosition.X = mapCanvas.ActualWidth - ballDiameter;
                }

                if (newPosition.Y < 0)
                {
                    newPosition.Y = 0;
                }
                else if (newPosition.Y > mapCanvas.ActualHeight - ballDiameter)
                {
                    newPosition.Y = mapCanvas.ActualHeight - ballDiameter;
                }

                // Check for wall hit and change ball direction
                if (newPosition.X >= mapCanvas.ActualWidth - ballDiameter || newPosition.X <= 0)
                {
                    direction.X *= -1;
                    wallHits++;
                }
                if (newPosition.Y >= mapCanvas.ActualHeight - ballDiameter || newPosition.Y <= 0)
                {
                    direction.Y *= -1;
                    wallHits++;
                }

                foreach (var hole in holes)
                {
                    if (BallHitHole(ballEllipse, hole))
                    {
                        ballMoving = false;
                        CompositionTarget.Rendering -= Render;
                    }
                }

                SetLocation(ballEllipse,newPosition);
            }
            else
            {
                CompositionTarget.Rendering -= Render;
            }
        }

        private void SetLocation(FrameworkElement child, Point point)
        {
            Canvas.SetLeft(child, point.X);
            Canvas.SetTop(child, point.Y);
        }

        private Point GetLocation(FrameworkElement frameworkElement)
        {
            return new Point(Canvas.GetLeft(frameworkElement), Canvas.GetTop(frameworkElement));
        }

        private bool BallHitHole(Ellipse ball, Ellipse hole)
        {

            
            Point centerOfBall = new Point(
                  Canvas.GetLeft(ball) + (ball.Width / 2),
                  Canvas.GetTop(ball) + (ball.Height / 2));

            Point centerOfHole = new Point(
                  Canvas.GetLeft(hole) + (hole.Width / 2),
                  Canvas.GetTop(hole) + (hole.Height / 2));

            double _xRadius = hole.Width / 2;
            double _yRadius = hole.Height / 2;


            if (_xRadius <= 0.0 || _yRadius <= 0.0)
                return false;

            Point normalized = new Point(centerOfHole.X - centerOfBall.X,
                                         centerOfHole.Y - centerOfBall.Y);

            return ((double)(normalized.X * normalized.X)
                     / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
                <= 1.0;
  
        }

        private void MapCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!ballMoving)
            {
                if (direction == default(Vector))
                {
                    var ellipsePosition = GetLocation(ballEllipse);
                    var mousePosition = Mouse.GetPosition(mapCanvas);
                    direction = new Vector(mousePosition.X, mousePosition.Y) -
                                new Vector(ellipsePosition.X, ellipsePosition.Y);
                    direction.Normalize();
                }

                wallHits = 0;
                ballMoving = true;
                CompositionTarget.Rendering += Render;
            }
            else
            {
                ballMoving = false;
                CompositionTarget.Rendering -= Render;
            }
        }
    }
}
