namespace Billapong.Host
{
    using System.Data.Entity;
    using DataAccess;
    using DataAccess.Initialize;

    /// <summary>
    /// Starting point of the host program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            // add database initializer
            Database.SetInitializer<BillapongDbContext>(new BillapongDbInitializer());

            // start wcf host
            var host = new Host();
            host.Start();
        }
    }
}
