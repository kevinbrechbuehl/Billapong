namespace Billapong.Tests.Common
{
    using System.Data.Entity;
    using Billapong.DataAccess;
    using Billapong.DataAccess.Initialize;

    /// <summary>
    /// Base class for all unit tests.
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        public TestBase()
        {
            // add database initializer
            Database.SetInitializer<BillapongDbContext>(new BillapongDbInitializer());
        }
    }
}
