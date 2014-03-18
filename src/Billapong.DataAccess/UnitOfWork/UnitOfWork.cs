namespace Billapong.DataAccess.UnitOfWork
{
    using System;
    using Model.Map;
    using Repository;

    /// <summary>
    /// Unit of work pattern implementation.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The member to save if instances is disposed or not.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The database context
        /// </summary>
        private BillapongDbContext context;

        /// <summary>
        /// The map repository
        /// </summary>
        private IRepository<Map> mapRepository;

        /// <summary>
        /// The window repository
        /// </summary>
        private IRepository<Window> windowRepository;

        /// <summary>
        /// The hole repository
        /// </summary>
        private IRepository<Hole> holeRepository;

        /// <summary>
        /// The high score repository
        /// </summary>
        private IRepository<HighScore> highScoreRepository; 

        /// <summary>
        /// Gets the map repository.
        /// </summary>
        /// <value>
        /// The map repository.
        /// </value>
        public IRepository<Map> MapRepository
        {
            get { return this.mapRepository ?? (this.mapRepository = new Repository<Map>(this.Context)); }
        }

        /// <summary>
        /// Gets the window repository.
        /// </summary>
        /// <value>
        /// The window repository.
        /// </value>
        public IRepository<Window> WindowRepository
        {
            get { return this.windowRepository ?? (this.windowRepository = new Repository<Window>(this.Context)); }
        }

        /// <summary>
        /// Gets the hole repository.
        /// </summary>
        /// <value>
        /// The hole repository.
        /// </value>
        public IRepository<Hole> HoleRepository
        {
            get { return this.holeRepository ?? (this.holeRepository = new Repository<Hole>(this.Context)); }
        }

        /// <summary>
        /// Gets the high score repository.
        /// </summary>
        /// <value>
        /// The high score repository.
        /// </value>
        public IRepository<HighScore> HighScoreRepository
        {
            get { return this.highScoreRepository ?? (this.highScoreRepository = new Repository<HighScore>(this.Context)); }
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected BillapongDbContext Context
        {
            get { return this.context ?? (this.context = new BillapongDbContext()); }
        }

        /// <summary>
        /// Saves changes to the data provider.
        /// </summary>
        public virtual void Save()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
