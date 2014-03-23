namespace Billapong.GameConsoleTest.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;
    using System.Windows;
    using Billapong.GameConsole.Configuration;
    using Billapong.GameConsole.Game;
    using Billapong.GameConsole.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the GameHelpers class
    /// </summary>
    [TestClass]
    public class GameHelpersTest
    {
        #region GetRandomWindow Method

        /// <summary>
        /// Gets a random window from a list of multiple windows.
        /// </summary>
        [TestMethod]
        public void GetRandomWindowFromMultipleWindows()
        {
            // arrange
            var windows = new List<Window>();
            for (var windowCount = 0; windowCount < 5; windowCount++)
            {
                windows.Add(new Window());
            }

            // act
            var randomWindow = GameHelpers.GetRandomWindow(windows);

            // assert
            Assert.IsNotNull(randomWindow);
            CollectionAssert.Contains(windows, randomWindow);
        }

        /// <summary>
        /// Gets a random window from a list with a single window.
        /// </summary>
        [TestMethod]
        public void GetRandomWindowFromSingleWindow()
        {
            // arrange
            var windows = new List<Window> { new Window() };

            // act
            var randomWindow = GameHelpers.GetRandomWindow(windows);

            // assert
            Assert.IsNotNull(randomWindow);
            CollectionAssert.Contains(windows, randomWindow);
        }

        /// <summary>
        /// Gets a random window from an empty window list.
        /// </summary>
        [TestMethod]
        public void GetRandomWindowFromEmptyList()
        {
            // arrange
            var windows = new List<Window>();

            // act
            var randomWindow = GameHelpers.GetRandomWindow(windows);

            // assert
            Assert.IsNull(randomWindow);
        }

        #endregion

        #region GetRandomBallPosition Method

        /// <summary>
        /// Gets a random ball position from a window.
        /// </summary>
        [TestMethod]
        public void GetRandomBallPositionFromWindow()
        {
            // arrange
            var window = new Window();
            for (var holeCount = 0; holeCount < 5; holeCount++)
            {
                var hole = new Hole { X = 0, Y = holeCount };
                window.Holes.Add(hole);
            }

            // act
            var randomBallPosition = GameHelpers.GetRandomBallPosition(window);

            // assert
            Assert.IsNotNull(randomBallPosition);
            Assert.IsFalse(window.Holes.Any(hole => hole.X == Convert.ToInt32(randomBallPosition.Value.X) && hole.Y == Convert.ToInt32(randomBallPosition.Value.Y)));
        }

        /// <summary>
        /// Gets a random ball position from a window with holes in every grid position. This should return no possible ball position
        /// </summary>
        [TestMethod]
        public void GetRandomBallPositionFromWindowWithMaximumHoles()
        {
            // arrange
            var window = new Window();
            for (var rowCount = 0; rowCount < GameConfiguration.GameGridSize; rowCount++)
            {
                for (var columnCount = 0; columnCount < GameConfiguration.GameGridSize; columnCount++)
                {
                    var hole = new Hole { X = rowCount, Y = columnCount };
                    window.Holes.Add(hole);
                }
            }

            // act
            var randomBallPosition = GameHelpers.GetRandomBallPosition(window);

            // assert
            Assert.IsNull(randomBallPosition);
        }

        /// <summary>
        /// Gets a random ball position without a initialized window. This should return no ball position
        /// </summary>
        [TestMethod]
        public void GetRandomBallPositionFromNullWindow()
        {
            // arrange
            Window window = null;

            // act
            var randomBallPosition = GameHelpers.GetRandomBallPosition(window);

            // assert
            Assert.IsNull(randomBallPosition);
        }

        #endregion

        #region GetRandomBallDirection Method

        /// <summary>
        /// Gets the random ball direction with a not initialized window. ArgumentNullException is expected
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRandomBallDirectionWithNullWindow()
        {
            // arrange 
            Window window = null;
            var ballPosition = new Point(100, 100);

            // act
            var result = GameHelpers.GetRandomBallDirection(window, ballPosition);
         
            // assert
            Assert.Fail("No ArgumentNullExecption was thrown.");
        }

        /// <summary>
        /// Gets the random ball direction from a window without holes.
        /// </summary>
        [TestMethod]
        public void GetRandomBallDirectionFromWindowWithoutHole()
        {
            // arrange 
            var window = new Window();
            var ballPosition = new Point(100, 100);

            // act
            var result = GameHelpers.GetRandomBallDirection(window, ballPosition);

            // assert
            Assert.AreNotEqual(default(Vector), result);
        }

        /// <summary>
        /// Gets the random ball direction from a window with holes.
        /// </summary>
        [TestMethod]
        public void GetRandomBallDirectionFromWindowWithHoles()
        {
            // arrange 
            var window = new Window();
            var ballPosition = new Point(100, 100);

            for (var holeCount = 0; holeCount < 5; holeCount++)
            {
                var hole = new Hole { X = 0, Y = holeCount };
                window.Holes.Add(hole);
            }

            // act
            var result = GameHelpers.GetRandomBallDirection(window, ballPosition);

            // assert
            Assert.AreNotEqual(default(Vector), result);
        }

        /// <summary>
        /// Gets the random ball direction from a window with holes surrounding the whole windows.
        /// </summary>
        [TestMethod]
        public void GetRandomBallDirectionFromWindowWithFullSurroundingHoles()
        {
            // arrange 
            var window = new Window();
            var ballPosition = new Point(100, 100);

            for (var holeCount = 0; holeCount < 15; holeCount++)
            {
                var upperBorderHole = new Hole { X = holeCount, Y = 0 };
                var lowerBorderHole = new Hole { X = holeCount, Y = 14 };
                window.Holes.Add(upperBorderHole);
                window.Holes.Add(lowerBorderHole);
            }

            for (var holeCount = 1; holeCount < 14; holeCount++)
            {
                var leftBorderHole = new Hole { X = 0, Y = holeCount };
                var rightBorderHole = new Hole { X = 14, Y = holeCount };
                window.Holes.Add(leftBorderHole);
                window.Holes.Add(rightBorderHole);
            }

            // act
            var result = GameHelpers.GetRandomBallDirection(window, ballPosition, 10);

            // assert
            Assert.AreNotEqual(default(Vector), result);
        }

        #endregion

        #region GetBallPositionFromGridCoordinates Method

        /// <summary>
        /// Gets the upper left corner ball position from grid coordinates.
        /// </summary>
        [TestMethod]
        public void GetUpperLeftCornerBallPositionFromGridCoordinates()
        {
            // arrange
            var ballGridPosition = new Point(0, 0);

            // act
            var result = GameHelpers.GetBallPositionFromGridCoordinates(ballGridPosition);

            // assert   
            Assert.AreEqual(6.66666, result.X, 0.00001);
            Assert.AreEqual(6.66666, result.Y, 0.00001);
        }

        /// <summary>
        /// Gets the lower right corner ball position from grid coordinates.
        /// </summary>
        [TestMethod]
        public void GetLowerRightCornerBallPositionFromGridCoordinates()
        {
            // arrange
            var ballGridPosition = new Point(14, 14);

            // act
            var result = GameHelpers.GetBallPositionFromGridCoordinates(ballGridPosition);

            // assert   
            Assert.AreEqual(193.33333, result.X, 0.00001);
            Assert.AreEqual(193.33333, result.Y, 0.00001);
        }

        #endregion
    }
}
