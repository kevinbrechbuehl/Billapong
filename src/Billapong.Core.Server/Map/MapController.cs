namespace Billapong.Core.Server.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Model.Map;
    using DataAccess.Repository;

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
        /// <param name="onlyPlayable">if set to <c>true</c> only currently playable maps will be returned.</param>
        /// <returns>
        /// Available maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps(bool onlyPlayable = false)
        {
            return this.repository.Get(includeProperties: "Windows, Windows.Holes").ToList();
        }
    }
}
