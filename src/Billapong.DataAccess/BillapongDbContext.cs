namespace Billapong.DataAccess
{
    using Model.Tracing;
    using System.Data.Entity;

    /// <summary>
    /// The database context for Billapong
    /// </summary>
    public class BillapongDbContext : DbContext
    {

        #region Singleton Implementation

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static BillapongDbContext Instance { get; private set; }

        /// <summary>
        /// Initializes the <see cref="BillapongDbContext"/> class.
        /// </summary>
        static BillapongDbContext()
        {
            Instance = new BillapongDbContext();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="BillapongDbContext"/> class from being created.
        /// </summary>
        private BillapongDbContext()
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>
        /// The log messages.
        /// </value>
        public DbSet<LogMessage> LogMessages { get; set; }
    }
}
