namespace Billapong.DataAccess
{
    using System.Data.Entity;
    using Model.Map;
    using Model.Tracing;

    /// <summary>
    /// The database context for Billapong
    /// </summary>
    public class BillapongDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>
        /// The log messages.
        /// </value>
        public DbSet<LogMessage> LogMessages { get; set; }

        /// <summary>
        /// Gets or sets the maps.
        /// </summary>
        /// <value>
        /// The maps.
        /// </value>
        public DbSet<Map> Maps { get; set; }
    }
}
