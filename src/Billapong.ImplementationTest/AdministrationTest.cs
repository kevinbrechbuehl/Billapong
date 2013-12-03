using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billapong.Implementation;

namespace Billapong.ImplementationTest
{
    [TestClass]
    public class AdministrationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new AdministrationService();
            var testName = service.Test("Kevin");
            Assert.IsNotNull(testName);
            StringAssert.Equals(testName, "Hello Kevin");
        }
    }
}
