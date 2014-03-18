using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditorTest.Services
{
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Server.Services;
    using Billapong.MapEditor.Services;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MapEditorServiceClientTest : TestBase
    {
        private readonly static ServiceHost MapEditorService = new ServiceHost(typeof(MapEditorService));

        private readonly static ServiceHost AuthenticationService = new ServiceHost(typeof(AuthenticationService));

        [ClassInitialize]
        public static void StartService(TestContext testContext)
        {
            MapEditorService.Open();
            AuthenticationService.Open();
        }

        [TestMethod]
        public void GetMapsTest()
        {
            // arrange
            var authenticatioProxy = new AuthenticationServiceClient();
            var sessionId = authenticatioProxy.Login("editor", "editor", Role.Editor);
            var mapEditorProxy = new MapEditorServiceClient(sessionId);

            // act
            var maps = mapEditorProxy.GetMaps().ToList();

            // asset
            Assert.IsTrue(maps.Count > 1);
            Assert.IsTrue(maps.First().Windows.Count == 1);
            Assert.IsTrue(maps.First().Windows.First().Holes.Count == 3);
        }

        [ClassCleanup]
        public static void EndService()
        {
            if (MapEditorService.State == CommunicationState.Opened)
            {
                MapEditorService.Close();
            }

            if (AuthenticationService.State == CommunicationState.Opened)
            {
                AuthenticationService.Close();
            }
        }
    }
}
