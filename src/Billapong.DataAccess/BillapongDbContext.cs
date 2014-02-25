namespace Billapong.DataAccess
{
    using System.Data.Entity;
    using Model.GamePlay;
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

        /// <summary>
        /// Gets or sets the high scores.
        /// </summary>
        /// <value>
        /// The high scores.
        /// </value>
        public DbSet<HighScore> HighScores { get; set; }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>().HasMany(x => x.Windows).WithRequired().WillCascadeOnDelete();
            modelBuilder.Entity<Window>().HasMany(x => x.Holes).WithRequired().WillCascadeOnDelete();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
