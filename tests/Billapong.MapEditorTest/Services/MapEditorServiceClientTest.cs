using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditorTest.Services
{
    using System.ServiceModel;
    using Billapong.Core.Server.Services;
    using Billapong.MapEditor.Services;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MapEditorServiceClientTest : TestBase
    {
        private readonly static ServiceHost MapEditorService = new ServiceHost(typeof(MapEditorService));

        [ClassInitialize]
        public static void StartService(TestContext testContext)
        {
            MapEditorService.Open();
        }

        [ClassCleanup]
        public static void EndService()
        {
            if (MapEditorService.State == CommunicationState.Opened)
            {
                MapEditorService.Close();
            }
        }

        [TestMethod]
        public void GetMapsTest()
        {
            // arrange
            // todo (breck1): fix test because we need a valid session id here
            var service = new MapEditorServiceClient(Guid.NewGuid());

            // act
            var maps = service.GetMaps().ToList();

            // asset
            Assert.IsTrue(maps.Count > 1);
            Assert.IsTrue(maps.First().Windows.Count == 1);
            Assert.IsTrue(maps.First().Windows.First().Holes.Count == 3);
        }
    }
}
