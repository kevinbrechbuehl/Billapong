using System;
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
        private Vector direction = new Vector(0, 0);
        private const int EllipseDiameter = 20;
        private bool ballMoving;
        private const float Speed = 10;
        private int wallHits;

        public GameWindow()
        {
            InitializeComponent();

            // Add ball to map
            ballEllipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Black),
                Height = EllipseDiameter,
                Width = EllipseDiameter
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
                Point newPosition = currentPosition + (direction*Speed);

                // Fix border positions if out of bounds
                if (newPosition.X < 0)
                {
                    newPosition.X = 0;
                }
                else if (newPosition.X > mapCanvas.ActualWidth - EllipseDiameter)
                {
                    newPosition.X = mapCanvas.ActualWidth - EllipseDiameter;
                }

                if (newPosition.Y < 0)
                {
                    newPosition.Y = 0;
                }
                else if (newPosition.Y > mapCanvas.Height - EllipseDiameter)
                {
                    newPosition.Y = mapCanvas.Height - EllipseDiameter;
                }

                // Check for wall hit and change ball direction
                if (newPosition.X >= mapCanvas.ActualWidth - EllipseDiameter || newPosition.X <= 0)
                {
                    direction.X *= -1;
                    wallHits++;
                }
                if (newPosition.Y >= mapCanvas.ActualHeight - EllipseDiameter || newPosition.Y <= 0)
                {
                    direction.Y *= -1;
                    wallHits++;
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

        private void MapCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var ellipsePosition = GetLocation(ballEllipse);
            var mousePosition = Mouse.GetPosition(mapCanvas);
            direction = new Vector(mousePosition.X, mousePosition.Y) - new Vector(ellipsePosition.X, ellipsePosition.Y);
            direction.Normalize();

            if (!ballMoving)
            {
                wallHits = 0;
                ballMoving = true;
                CompositionTarget.Rendering += Render;
            }
        }
    }
}
