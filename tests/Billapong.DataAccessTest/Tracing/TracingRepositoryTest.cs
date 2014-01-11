namespace Billapong.DataAccessTest.Tracing
{
    using System;
    using System.Linq;
    using DataAccess.Model.Tracing;
    using DataAccess.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests The LogMessage class
    /// </summary>
    [TestClass]
    public class TracingRepositoryTest
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
                LogLevel = 1,
                Message = "This is a test debug message.",
                Component = "Unit Test",
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
