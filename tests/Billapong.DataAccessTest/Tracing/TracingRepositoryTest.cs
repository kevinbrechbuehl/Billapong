namespace Billapong.DataAccessTest.Tracing
{
    using System;
    using System.Linq;
    using Billapong.Tests.Common;
    using DataAccess.Model.Tracing;
    using DataAccess.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests The LogMessage class
    /// </summary>
    [TestClass]
    public class TracingRepositoryTest : TestBase
    {
        /// <summary>
        /// Tests the repository
        /// </summary>
        [TestMethod]
        public void LogMessageRepositoryTest()
        {
            // arrange
            var repository = new Repository<LogMessage>();
            var count = repository.Get().Count();

            var message = new LogMessage
            {
                LogLevel = (int)Contract.Data.Tracing.LogLevel.Debug,
                Message = "This is a test debug message.",
                Component = Contract.Data.Tracing.Component.Server.ToString(),
                Sender = "Unit Test Method",
                Timestamp = DateTime.Now
            };

            // act
            repository.Add(message);
            repository.Save();
            var newCount = repository.Get().Count();

            // assert
            Assert.IsTrue(count + 1 == newCount, "Log message was not saved correctly.");
        }
    }
}
