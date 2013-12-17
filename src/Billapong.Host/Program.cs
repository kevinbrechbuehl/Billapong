namespace Billapong.Host
{
    using System.Data.Entity;
    using DataAccess;
    using DataAccess.Initialize;

    class Program
    {
        static void Main(string[] args)
        {
            // add database initializer
            Database.SetInitializer<BillapongDbContext>(new BillapongDbInitializer());

            // start wcf host
            var host = new Host();
            host.Start();
        }
    }
}
