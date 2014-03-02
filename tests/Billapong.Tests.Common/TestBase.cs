namespace Billapong.Tests.Common
{
    using System.Data.Entity;

    using Billapong.DataAccess;
    using Billapong.DataAccess.Initialize;

    public class TestBase
    {
        public TestBase()
        {
            // add database initializer
            Database.SetInitializer<BillapongDbContext>(new BillapongDbInitializer());
        }
    }
}
