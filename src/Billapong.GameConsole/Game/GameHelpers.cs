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

            var validBallPositions = new List<int[]>();

            for (var row = 0; row < GameConfiguration.GameGridSize; row++)
            {
                for (var column = 0; column < GameConfiguration.GameGridSize; column++)
                {
                    if (window.Holes.FirstOrDefault(hole => hole.X == row && hole.Y == column) == null)
                    {
                        validBallPositions.Add(new[] { row, column });
                    }
                }
            }

            if (!validBallPositions.Any()) return null;

            var random = new Random(DateTime.Now.GetHashCode());
            var randomBallPosition = validBallPositions.ElementAt(random.Next(0, validBallPositions.Count));
            
            return new Point(randomBallPosition[0], randomBallPosition[1]);
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
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

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
                var intersectionTestPoint = ballPosition + (direction * 10);
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
                        Tracer.Debug(string.Format("GameHelpers :: GetRandomBallDirection :: Found an intersection between click position {0}, ball position {1}, direction {2} and hole position {3}", clickPosition, ballPosition, direction, hole.CenterPosition));
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

        /// <summary>
        /// Calculates the ball position based on grid coordinates.
        /// </summary>
        /// <param name="gridCoordinates">The grid coordinates.</param>
        /// <returns>The calculated point</returns>
        public static Point GetBallPositionFromGridCoordinates(Point gridCoordinates)
        {
            var positionX = (GameConfiguration.GameGridElementSize * gridCoordinates.X) +
                            ((GameConfiguration.GameGridElementSize - GameConfiguration.BallDiameter) / 2) + (GameConfiguration.BallDiameter / 2);
            var positionY = (GameConfiguration.GameGridElementSize * gridCoordinates.Y) +
                            ((GameConfiguration.GameGridElementSize - GameConfiguration.BallDiameter) / 2) + (GameConfiguration.BallDiameter / 2);

            return new Point(positionX, positionY);
        }
    }
}
