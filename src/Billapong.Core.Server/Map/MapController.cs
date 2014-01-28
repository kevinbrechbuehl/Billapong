namespace Billapong.Core.Server.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Model.Map;
    using DataAccess.Repository;

    /// <summary>
    /// The map controller
    /// </summary>
    public class MapController
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository<Map> repository;

        #region Singleton Implementation

        /// <summary>
        /// Initializes static members of the <see cref="MapController"/> class.
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

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <value>
        /// The current instance.
        /// </value>
        public static MapController Current { get; private set; }

        #endregion

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <param name="onlyPlayable">if set to <c>true</c> only currently playable maps will be returned.</param>
        /// <returns>
        /// Available maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps(bool onlyPlayable = false)
        {
            var maps = this.repository.Get(includeProperties: "Windows, Windows.Holes");
            if (onlyPlayable)
            {
                maps = maps.Where(map => map.IsPlayable);
            }
    
            return maps.ToList();
        }

        /// <summary>
        /// Gets the map by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="onlyPlayable">if set to <c>true</c> only currently playable maps will be returned.</param>
        /// <returns>Map object from the database</returns>
        public Map GetMapById(long id, bool onlyPlayable = false)
        {
            var maps = this.repository.Get(filter: map => map.Id == id, includeProperties: "Windows, Windows.Holes");
            if (onlyPlayable)
            {
                maps = maps.Where(map => map.IsPlayable);
            }
            
            return maps.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteMap(long id)
        {
            this.repository.Remove(id);
            this.repository.Save();
        }
    }
}
