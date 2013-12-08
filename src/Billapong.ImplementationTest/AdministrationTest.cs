using System;
using System.Linq;
using Billapong.Contract.Service;
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
            IAdministrationService service = new AdministrationService();
            var maps = service.GetMaps();
            Assert.IsNotNull(maps);
            Assert.IsTrue(maps.Any());
        }
    }
}
