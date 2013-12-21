namespace Billapong.DataAccess
{
    using Model.Editor;
    using Model.Tracing;
    using System.Data.Entity;

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
