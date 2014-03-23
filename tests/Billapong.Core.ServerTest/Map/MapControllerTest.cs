namespace Billapong.Core.ServerTest.Map
{
    using Billapong.Core.Server.Map;
    using Billapong.DataAccess.Repository;
    using Billapong.Tests.Common;
    using DataAccess.Model.Map;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the map controller.
    /// </summary>
    [TestClass]
    public class MapControllerTest : TestBase
    {
        /// <summary>
        /// Test if the name of a map can be updated over the controller.
        /// </summary>
        [TestMethod]
        public void UpdateNameTest()
        {
            // arrange
            const int MapId = 1;
            const string NewName = "New Map Name";

            var nameBefore = new Repository<Map>().GetById(MapId).Name;

            // act
            MapController.Current.UpdateName(MapId, NewName);
            var nameAfter = new Repository<Map>().GetById(MapId).Name;
            MapController.Current.UpdateName(MapId, nameBefore);
            var nameAtEnd = new Repository<Map>().GetById(MapId).Name;

            // asset
            Assert.AreEqual(NewName, nameAfter, "Map name could not me updated");
            Assert.AreEqual(nameBefore, nameAtEnd, "Map name could not be resetted to the beginning value");
        }
    }
}
