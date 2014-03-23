namespace Billapong.Core.ServerTest.Services
{
    using System.Linq;

    using Billapong.Tests.Common;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Services;

    /// <summary>
    /// Tests for game console service
    /// </summary>
    [TestClass]
    public class GameConsoleServiceTest : TestBase
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        [TestMethod]
        public void GetMapsTest()
        {
            // arrange
            var service = new GameConsoleService();

            // act
            var maps = service.GetMaps().ToList();

            // asset
            Assert.IsTrue(maps.Count > 1);
            Assert.IsTrue(maps.First().Windows.Count == 1);
            Assert.IsTrue(maps.First().Windows.First().Holes.Count == 3);
        }

        /// <summary>
        /// Gets the map by id.
        /// </summary>
        [TestMethod]
        public void GetMapByIdTest()
        {
            // arrange
            var service = new GameConsoleService();
            var firstMap = service.GetMaps().First();

            // act
            var map = service.GetMapById(firstMap.Id);

            // asset
            Assert.IsNotNull(map);
            Assert.AreEqual(firstMap.Id, map.Id);
            Assert.IsTrue(map.Windows.Count > 0);
            Assert.AreEqual(firstMap.Windows.Count, map.Windows.Count);
            Assert.IsTrue(map.Windows.First().Holes.Count > 0);
            Assert.AreEqual(firstMap.Windows.First().Holes.Count, map.Windows.First().Holes.Count);
        }
    }
}
