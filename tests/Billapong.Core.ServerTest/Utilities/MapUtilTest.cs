namespace Billapong.Core.ServerTest.Utilities
{
    using System.Collections.Generic;
    using Billapong.Core.Server.Utilities;
    using Billapong.DataAccess.Model.Map;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the map helper methods.
    /// </summary>
    [TestClass]
    public class MapUtilTest
    {
        /// <summary>
        /// Map without any windows should not be valid.
        /// </summary>
        [TestMethod]
        public void MapWithoutWindowsTest()
        {
            // arrange
            var map = new Map();

            // act
            var isValid = MapUtil.IsPlayable(map);

            // assert
            Assert.IsFalse(isValid, "Map with no windows should not be valid");
        }

        /// <summary>
        /// Map with one window and one hole should be valid.
        /// </summary>
        [TestMethod]
        public void MapWithOneWindowWithHolesTest()
        {
            // arrange
            var map = new Map();
            map.Windows.Add(new Window { Holes = new List<Hole> { new Hole() } });

            // act
            var isValid = MapUtil.IsPlayable(map);

            // assert
            Assert.IsTrue(isValid, "Map with 1 window should be valid");
        }

        /// <summary>
        /// Map with only one window and without a hole should not be valid.
        /// </summary>
        [TestMethod]
        public void MapWithOneWindowWithoutHolesTest()
        {
            // arrange
            var map = new Map();
            map.Windows.Add(new Window());

            // act
            var isValid = MapUtil.IsPlayable(map);

            // assert
            Assert.IsFalse(isValid, "Map without holes should not be valid");
        }

        /// <summary>
        /// Map with multiple windows which are all directly connected to each other should be valid
        /// </summary>
        [TestMethod]
        public void MapWithMultipleValidWindowsTest()
        {
            // arrange
            var map = new Map();
            map.Windows.Add(new Window { Id = 1, X = 0, Y = 0, Holes = new List<Hole> { new Hole() } });
            map.Windows.Add(new Window { Id = 2, X = 1, Y = 0 });
            map.Windows.Add(new Window { Id = 3, X = 2, Y = 0 });

            // act
            var isValid = MapUtil.IsPlayable(map);

            // assert
            Assert.IsTrue(isValid, "Map with connected neighbor windows should be valid");
        }

        /// <summary>
        /// Map with multiple windows which are not directly connected to each other should not be valid.
        /// </summary>
        [TestMethod]
        public void MapWithMultipleInvalidWindowsTest()
        {
            // arrange
            var map = new Map();
            map.Windows.Add(new Window { Id = 1, X = 0, Y = 0, Holes = new List<Hole> { new Hole() } });
            map.Windows.Add(new Window { Id = 2, X = 1, Y = 0 });
            map.Windows.Add(new Window { Id = 3, X = 2, Y = 1 });

            // act
            var isValid = MapUtil.IsPlayable(map);

            // assert
            Assert.IsFalse(isValid, "Map without connected neighbor windows should not be valid");
        }
    }
}
