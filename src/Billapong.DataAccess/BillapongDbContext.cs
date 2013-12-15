namespace Billapong.DataAccess
{
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
    }
}
