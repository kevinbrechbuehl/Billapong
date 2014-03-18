namespace Billapong.MapEditorTest.Services
{
    using System.Linq;
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Core.Client.Authentication;
    using Billapong.Core.Server.Services;
    using Billapong.MapEditor.Services;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the map editor with a service client.
    /// </summary>
    [TestClass]
    public class MapEditorServiceClientTest : TestBase
    {
        /// <summary>
        /// The map editor service
        /// </summary>
        private static readonly ServiceHost MapEditorService = new ServiceHost(typeof(MapEditorService));

        /// <summary>
        /// The authentication service
        /// </summary>
        private static readonly ServiceHost AuthenticationService = new ServiceHost(typeof(AuthenticationService));

        /// <summary>
        /// Starts the service hosts.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void StartService(TestContext testContext)
        {
            MapEditorService.Open();
            AuthenticationService.Open();
        }

        /// <summary>
        /// Ends the service hosts.
        /// </summary>
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

        /// <summary>
        /// Gets the maps.
        /// </summary>
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
    }
}
