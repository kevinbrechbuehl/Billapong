namespace Billapong.GameConsoleTest.Game
{
    using System;
    using System.Windows;
    using Billapong.GameConsole.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CalculationHelpersTest
    {
        #region DistanceTo Method

        /// <summary>
        /// Calculates the distance between two superposed points.
        /// </summary>
        [TestMethod]
        public void CalculateSuperPosedPointsDistance()
        {
            // arrange
            var startPoint = new Point(30, 30);
            var endPoint = new Point(30, 30);

            // act
            var distance = startPoint.DistanceTo(endPoint);

            // assert
            Assert.AreEqual(0, distance);
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        [TestMethod]
        public void CalculatePointsDistance()
        {
            // arrange
            var startPoint = new Point(30, 47);
            var endPoint = new Point(70, 50);

            // act
            var distance = startPoint.DistanceTo(endPoint);

            // assert
            Assert.AreEqual(40.1123422, Math.Round(distance, 7));
        }

        #endregion

        #region GetLineIntersection Method

        [TestMethod]
        public void CalculateLineIntersectionBetweenTwoLines()
        {
            // arrange
            var firstLineStartPoint = new Point(0, 0);
            var firstLineEndPoint = new Point(300, 300);
            var secondLineStartPoint = new Point(0, 300);
            var secondLineEndPoint = new Point(300, 0);

            // act
            var intersection = CalculationHelpers.GetLineIntersection(
                firstLineStartPoint,
                firstLineEndPoint,
                secondLineStartPoint,
                secondLineEndPoint);

            // assert
            Assert.IsNotNull(intersection);
            Assert.AreEqual(new Point(150, 150), intersection.Value);
        }

        /// <summary>
        /// Calculates the line intersection between two superposed zero length lines. That cannot work because lines with zero length are not lines
        /// </summary>
        [TestMethod]
        public void CalculateLineIntersectionBetweenTwoSuperPosedZeroLengthLines()
        {
            // arrange
            var firstLineStartPoint = new Point(150, 150);
            var firstLineEndPoint = new Point(150, 150);
            var secondLineStartPoint = new Point(150, 150);
            var secondLineEndPoint = new Point(150, 150);

            // act
            var intersection = CalculationHelpers.GetLineIntersection(
                firstLineStartPoint,
                firstLineEndPoint,
                secondLineStartPoint,
                secondLineEndPoint);

            // assert
            Assert.IsNull(intersection);
        }

        [TestMethod]
        public void CalculateLineIntersectionBetweenTwoParallelLines()
        {
            // arrange
            var firstLineStartPoint = new Point(0, 0);
            var firstLineEndPoint = new Point(300, 300);
            var secondLineStartPoint = new Point(1, 1);
            var secondLineEndPoint = new Point(301, 301);

            // act
            var intersection = CalculationHelpers.GetLineIntersection(
                firstLineStartPoint,
                firstLineEndPoint,
                secondLineStartPoint,
                secondLineEndPoint);

            // assert
            Assert.IsNull(intersection);
        }

        [TestMethod]
        public void CalculateLineIntersectionBetweenTwoNotCrossingLines()
        {
            // arrange
            var firstLineStartPoint = new Point(50, 700);
            var firstLineEndPoint = new Point(500, 57);
            var secondLineStartPoint = new Point(49, 699);
            var secondLineEndPoint = new Point(2, 30);

            // act
            var intersection = CalculationHelpers.GetLineIntersection(
                firstLineStartPoint,
                firstLineEndPoint,
                secondLineStartPoint,
                secondLineEndPoint);

            // assert
            Assert.IsNull(intersection);
        }

        #endregion

        #region CalculateLineSphereIntersection

        /// <summary>
        /// Calculates the line sphere intersection which has two intersection points (run trough).
        /// </summary>
        [TestMethod]
        public void CalculateRunThroughLineSphereIntersection()
        {
            // arrange
            var sphereCenter = new Point(150, 150);
            const double SphereRadius = 5;
            var lineStartPoint = new Point(0, 0);
            var lineEndPoint = new Point(300, 300);

            Point? firstIntersection;
            Point? secondIntersection;

            // act
            var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                sphereCenter,
                SphereRadius,
                lineStartPoint,
                lineEndPoint,
                out firstIntersection,
                out secondIntersection);

            // assert
            Assert.AreEqual(2, intersection);
            Assert.IsNotNull(firstIntersection);
            Assert.AreEqual(146.4644660940672624, firstIntersection.Value.X, 0.00001);
            Assert.AreEqual(146.4644660940672624, firstIntersection.Value.Y, 0.00001);
            Assert.IsNotNull(secondIntersection);
            Assert.AreEqual(153.5355339059327376, secondIntersection.Value.X, 0.00001);
            Assert.AreEqual(153.5355339059327376, secondIntersection.Value.Y, 0.00001);
        }

        /// <summary>
        /// Calculates the line sphere intersection with only one intersection point
        /// </summary>
        [TestMethod]
        public void CalculateGrazeLineSphereIntersection()
        {
            // arrange
            var sphereCenter = new Point(150, 150);
            const double SphereRadius = 5;
            var lineStartPoint = new Point(0, 0);
            var lineEndPoint = new Point(300, 286.179770816163);

            Point? firstIntersection;
            Point? secondIntersection;

            // act
            var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                sphereCenter,
                SphereRadius,
                lineStartPoint,
                lineEndPoint,
                out firstIntersection,
                out secondIntersection);

            // assert
            Assert.AreEqual(1, intersection);
            Assert.IsNotNull(firstIntersection);
            Assert.AreEqual(153.45121834340819, firstIntersection.Value.X, 0.00001);
            Assert.AreEqual(146.38211498992513, firstIntersection.Value.Y, 0.00001);
            Assert.IsNull(secondIntersection);
        }

        /// <summary>
        /// Calculates the line sphere interection between the line and the sphere with no intersection point
        /// </summary>
        [TestMethod]
        public void CalculateLineSphereIntersectionWithNoIntersection()
        {
            // arrange
            var sphereCenter = new Point(150, 150);
            const double SphereRadius = 5;
            var lineStartPoint = new Point(0, 0);
            var lineEndPoint = new Point(300, 100);

            Point? firstIntersection;
            Point? secondIntersection;

            // act
            var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                sphereCenter,
                SphereRadius,
                lineStartPoint,
                lineEndPoint,
                out firstIntersection,
                out secondIntersection);

            // assert
            Assert.AreEqual(0, intersection);
            Assert.IsNull(firstIntersection);
            Assert.IsNull(secondIntersection);
        }

        /// <summary>
        /// Calculates the line sphere intersection when a line goes into the opposite direction.
        /// This was an actual problem in the project
        /// </summary>
        [TestMethod]
        public void CalculateLineSphereIntersectionWithOppositeDirection()
        {
            // arrange
            var sphereCenter = new Point(150, 150);
            const double SphereRadius = 5;
            var lineStartPoint = new Point(140, 140);
            var lineEndPoint = new Point(0, 0);

            Point? firstIntersection;
            Point? secondIntersection;

            // act
            var intersection = CalculationHelpers.CalculateLineSphereIntersection(
                sphereCenter,
                SphereRadius,
                lineStartPoint,
                lineEndPoint,
                out firstIntersection,
                out secondIntersection);

            // assert
            Assert.AreEqual(0, intersection);
            Assert.IsNull(firstIntersection);
            Assert.IsNull(secondIntersection);
        }

        #endregion
    }
}
