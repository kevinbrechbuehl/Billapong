namespace Billapong.GameConsoleTest.Service
{
    using System;
    using System.ServiceModel;
    using Billapong.Core.Server.Services;
    using Billapong.GameConsole.Service;
    using Billapong.Tests.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Integration tests for the GameConsoleService
    /// </summary>
    [TestClass]
    public class GameConsoleServiceClientTest : TestBase
    {
        /// <summary>
        /// The game console service
        /// </summary>
        private static readonly ServiceHost GameConsoleService = new ServiceHost(typeof(GameConsoleService));

        /// <summary>
        /// Starts the service host.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void StartService(TestContext testContext)
        {
            GameConsoleService.Open();
        }

        /// <summary>
        /// Ends the service.
        /// </summary>
        [ClassCleanup]
        public static void EndService()
        {
            if (GameConsoleService.State == CommunicationState.Opened)
            {
                GameConsoleService.Close();
            }
        }

        /// <summary>
        /// Checks whether the game with an invalid game identifier is running.
        /// </summary>
        [TestMethod]
        public void CheckIsGameRunningWithInvalidGameId()
        {
            // arrange
            var proxy = new GameConsoleServiceClient();

            // act
            var isGameRunning = proxy.IsGameRunning(new Guid());

            // assert
            Assert.IsFalse(isGameRunning);
        }
    }
}
