namespace Billapong.Core.Server.Editor
{
    using DataAccess.Model.Editor;
    using DataAccess.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public class MapController
    {
        #region Singleton Implementation

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static MapController Current { get; private set; }

        /// <summary>
        /// Initializes the <see cref="MapController"/> class.
        /// </summary>
        static MapController()
        {
            Current = new MapController();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MapController"/> class from being created.
        /// </summary>
        private MapController()
        {
            this.repository = new Repository<Map>();
        }

        #endregion

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository<Map> repository;

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <param name="includeUnplayable">if set to <c>true</c> unplayable maps will also returned.</param>
        /// <returns>Available maps in the database</returns>
        public IEnumerable<Map> GetMaps(bool includeUnplayable = false)
        {
            return this.repository.Get(includeProperties: "Windows, Windows.Holes").ToList();
        }
    }
}
