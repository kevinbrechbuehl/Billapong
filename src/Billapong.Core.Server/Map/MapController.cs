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
        private static readonly object WriterLockObject = new object();
        
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
            lock (WriterLockObject)
            {
                this.repository.Remove(id);
                this.repository.Save();
            }
        }

        public void SaveGeneral(long id, string name)
        {
            lock (WriterLockObject)
            {
                var map = this.GetMap(id);
                map.Name = name;
                this.repository.Save();
            }

            // todo (breck1): send callbacks
        }

        private Map GetMap(long id)
        {
            // todo (breck1): exception handling in whole class
            
            if (id > 0)
            {
                return this.repository.GetById(id);
            }

            var map = new Map
            {
                Name = "Unnamed",
                IsPlayable = false
            };

            this.repository.Add(map);
            this.repository.Save();
            return map;
        }
    }
}
