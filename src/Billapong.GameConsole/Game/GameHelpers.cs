namespace Billapong.GameConsole.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Billapong.Core.Client.Tracing;
    using Billapong.GameConsole.Configuration;
    using Window = Billapong.GameConsole.Models.Window;

    /// <summary>
    /// Provides some helper methods for the game flow
    /// </summary>
    public static class GameHelpers
    {
        /// <summary>
        /// Gets the random window out of the possible windows.
        /// </summary>
        /// <param name="possibleWindows">The possible windows.</param>
        /// <returns>The random selected window</returns>
        public static Window GetRandomWindow(IEnumerable<Window> possibleWindows)
        {
            var windows = possibleWindows as Window[] ?? possibleWindows.ToArray();
            if (!windows.Any())
            {
                return null;
            }

            var random = new Random(DateTime.Now.GetHashCode());
            return windows.ElementAt(random.Next(0, windows.Count()));
        }

        /// <summary>
        /// Gets random based ball position on a free grid position on the specified window
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>The ball position within the grid</returns>
        public static Point? GetRandomBallPosition(Window window)
        {
            if (window == null)
            {
                return null;
            }

            var positionFound = false;
            Point? ballPosition = null;
            var random = new Random(DateTime.Now.GetHashCode());
         
            // todo (mathp2): Possible endless loop if a map has a hole on every position. Performance is also not very good (many repetitions).
            while (!positionFound)
            {
                var x = random.Next(0, GameConfiguration.GameGridSize);
                var y = random.Next(0, GameConfiguration.GameGridSize);

                if (window.Holes.FirstOrDefault(hole => hole.X == x && hole.Y == y) != null)
                {
                    continue;
                }

                ballPosition = new Point(x, y);
                positionFound = true;
            }

            return ballPosition;
        }

        /// <summary>
        /// Gets a random ball direction which does not end up in a hole within the first direction.
        /// If no valid direction is found within the defined calculation steps, the last calculated direction is returned.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="ballPosition">The ball position.</param>
        /// <param name="maxCalculationSteps">The maximum calculation steps.</param>
        /// <returns>
        /// The direction
        /// </returns>
        public static Vector GetRandomBallDirection(Window window, Point ballPosition, int maxCalculationSteps = 2000)
        {
            var directionsCalculated = 0;
            var random = new Random(DateTime.Now.GetHashCode());
            while (true)
            {
                var clickPosition = new Point(
                    random.Next(0, GameConfiguration.GameWindowWidth),
                    random.Next(0, GameConfiguration.GameWindowHeight));

                // Set the direction and negate it, because the ball has to move away from the queue
                var direction = new Vector(clickPosition.X, clickPosition.Y)
                                - new Vector(ballPosition.X, ballPosition.Y);
                direction.Negate();
                directionsCalculated++;

                // The direction is valid if there are no holes in the window
                if (!window.Holes.Any())
                {
                    return direction;
                }

                // Create a fake point outside of the window in the ball direction
                var intersectionTestPoint = ballPosition + (direction * 1000);
                var intersectionFound = false;

                // Check for an intersection between the ball and a hole in the set direction
                foreach (var hole in window.Holes)
                {
                    Point? firstIntersection;
                    Point? secondIntersection;

                    var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                        hole.CenterPosition,
                        hole.Radius,
                        ballPosition,
                        intersectionTestPoint,
                        out firstIntersection,
                        out secondIntersection);

                    // Cancel the check because we have an intersection
                    if (intersection > 0)
                    {
                        Tracer.Debug(string.Format("GameHelpers :: GetRandomBallDirection :: Found an intersection between click position {0}, direction {1} and hole position {2}", clickPosition, direction, hole.CenterPosition));
                        intersectionFound = true;
                        break;
                    }
                }

                // Return the current direction if there was no valid direction within the defined tries
                if (directionsCalculated == maxCalculationSteps)
                {
                    Tracer.Debug(string.Format("GameHelpers :: GetRandomBallDirection :: Returning direction {0} because the amount of tries is reached", direction));
                    return direction;
                }

                // If we have a intersection, we need another try to find a valid direction
                if (intersectionFound)
                {
                    continue;
                }

                Tracer.Debug(string.Format("GameHelpers :: GetRandomBallDirection :: Returning direction {0} after {1} tries", direction, directionsCalculated));
                return direction;
            }
        }
    }
}
