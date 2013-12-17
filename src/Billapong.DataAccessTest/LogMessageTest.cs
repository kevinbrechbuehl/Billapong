using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Billapong.DataAccessTest
{
    using DataAccess.Model.Tracing;
    using DataAccess.Repository;
    using System.Linq;

    [TestClass]
    public class LogMessageTest
    {
        [TestMethod]
        public void RepositoryTest()
        {
            var repository = new Repository<LogMessage>();
            var count = repository.Get().Count();

            var message = new LogMessage
            {
                LogLevel = "Debug",
                Message = "This is a test debug message.",
                Component = "Unit Test",
                Timestamp = DateTime.Now
            };

            repository.Add(message);
            repository.Save();

            var newCount = repository.Get().Count();
            Assert.IsTrue(count + 1 == newCount, "Log message was not saved correctly.");
        }
    }
}
