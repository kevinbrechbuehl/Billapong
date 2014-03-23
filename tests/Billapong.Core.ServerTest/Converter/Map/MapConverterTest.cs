namespace Billapong.Core.ServerTest.Converter.Map
{
    using System.Linq;
    using Billapong.Core.Server.Converter.Map;
    using Billapong.DataAccess.Model.Map;
    using Billapong.DataAccess.Repository;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the map converter helper.
    /// </summary>
    [TestClass]
    public class MapConverterTest : TestBase
    {
        /// <summary>
        /// Test if an entity has correctly be converted to a contract.
        /// </summary>
        [TestMethod]
        public void MapToContractTest()
        {
            // arrange
            var repository = new Repository<Map>();
            var mapEntity = repository.Get().First();

            // act
            var mapContract = mapEntity.ToContract();

            // asset
            Assert.AreEqual(mapEntity.Id, mapContract.Id, "Id not equal");
            Assert.AreEqual(mapEntity.IsPlayable, mapContract.IsPlayable, "Is playable flag not equal");
            Assert.AreEqual(mapEntity.Name, mapContract.Name, "Name not equal");
            Assert.AreEqual(mapEntity.Windows.Count, mapContract.Windows.Count, "Number of windows not equal");
            Assert.AreEqual(mapEntity.Windows.Sum(window => window.Holes.Count), mapContract.Windows.Sum(window => window.Holes.Count), "Number of holes not equal");
        }
    }
}
