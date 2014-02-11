namespace Billapong.Core.Server.Map
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contract.Data.Map;
    using Contract.Service;
    using DataAccess.Model.Map;
    using DataAccess.Repository;
    using Map = DataAccess.Model.Map.Map;

    /// <summary>
    /// The map controller
    /// </summary>
    public class MapController
    {
        private static readonly object WriterLockObject = new object();

        private static readonly object CallbackLockObject = new object();

        private readonly IDictionary<long, MapEditor> editors = new Dictionary<long, MapEditor>();
        
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

        public void SaveGeneral(long id, string name, IMapEditorCallback callback)
        {
            Map map;
            lock (WriterLockObject)
            {
                map = this.GetMap(id);
                map.Name = name;
                this.repository.Save();
            }

            if (id == 0)
            {
                this.RegisterCallback(map.Id, callback);
            }

            // send the callback
            Task.Run(() => this.StartSaveGeneralCallback(map));
        }

        public void AddWindow(long mapId, int coordX, int coordY)
        {
            Map map;
            var window = new DataAccess.Model.Map.Window {X = coordX, Y = coordY};
            lock (WriterLockObject)
            {
                map = this.GetMap(mapId);
                if (map == null) return;

                map.Windows.Add(window);
                this.repository.Save();
            }

             // send the callback
            Task.Run(() => this.StartAddWindowCallback(mapId, window.Id, coordX, coordY));
        }

        public void RemoveWindow(long mapId, long windowId)
        {
            Map map;
            int coordX, coordY;
            lock (WriterLockObject)
            {
                map = this.GetMap(mapId);
                if (map == null) return;

                var window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                coordX = window.X;
                coordY = window.Y;
                map.Windows.Remove(window);
                this.repository.Save();
            }

             // send the callback
            Task.Run(() => this.StartRemoveWindowCallback(mapId, windowId, coordX, coordY));
        }

        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            var hole = new DataAccess.Model.Map.Hole {X = coordX, Y = coordY};
            DataAccess.Model.Map.Window window;
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                window.Holes.Add(hole);
                this.repository.Save();
            }

            // send the callback
            Task.Run(() => this.StartAddHoleCallback(mapId, windowId, window.X, window.Y, hole.Id, coordX, coordY));
        }

        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            DataAccess.Model.Map.Window window;
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                var hole = window.Holes.FirstOrDefault(gameHole => gameHole.Id == holeId);
                if (hole == null) return;

                window.Holes.Remove(hole);
                this.repository.Save();
            }

            // send the callback
            Task.Run(() => this.StartRemoveHoleCallback(mapId, windowId, window.X, window.Y, holeId));
        }

        public void RegisterCallback(long id, IMapEditorCallback callback)
        {
            lock (CallbackLockObject)
            {
                MapEditor editor;
                if (this.editors.ContainsKey(id))
                {
                    editor = this.editors[id];
                }
                else
                {
                    editor = new MapEditor();
                    this.editors.Add(id, editor);
                }

                editor.Callbacks.Add(callback);
            }
        }

        public void UnregisterCallback(long id, IMapEditorCallback callback)
        {
            lock (CallbackLockObject)
            {
                if (!this.editors.ContainsKey(id))
                {
                    return;
                }

                var editor = this.editors[id];
                editor.Callbacks.Remove(callback);
            }
        }

        private void StartAddWindowCallback(long mapId, long windowId, int coordX, int coordY)
        {
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(mapId) ? this.editors[mapId] : null;
                if (editor == null) return;

                // todo (breck1): refactor and verify callbacks on every method

                foreach (var callback in editor.Callbacks)
                {
                    callback.AddWindow(windowId, coordX, coordY);
                }
            }
        }

        private void StartRemoveWindowCallback(long mapId, long windowId, int coordX, int coordY)
        {
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(mapId) ? this.editors[mapId] : null;
                if (editor == null) return;

                // todo (breck1): refactor and verify callbacks on every method

                foreach (var callback in editor.Callbacks)
                {
                    callback.RemoveWindow(windowId, coordX, coordY);
                }
            }
        }

        private void StartAddHoleCallback(long mapId, long windowId, int windowX, int windowY, long holeId, int holeX, int holeY)
        {
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(mapId) ? this.editors[mapId] : null;
                if (editor == null) return;

                // todo (breck1): refactor and verify callbacks on every method

                foreach (var callback in editor.Callbacks)
                {
                    callback.AddHole(windowId, windowX, windowY, holeId, holeX, holeY);
                }
            }
        }

        private void StartRemoveHoleCallback(long mapId, long windowId, int windowX, int windowY, long holeId)
        {
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(mapId) ? this.editors[mapId] : null;
                if (editor == null) return;

                // todo (breck1): refactor and verify callbacks on every method

                foreach (var callback in editor.Callbacks)
                {
                    callback.RemoveHole(windowId, windowX, windowY, holeId);
                }
            }
        }

        private void StartSaveGeneralCallback(Map map)
        {
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(map.Id) ? this.editors[map.Id] : null;
                if (editor == null) return;

                // todo (breck1): refactor and verify callbacks on every method

                var data = new GeneralMapData {Id = map.Id, Name = map.Name};
                foreach (var callback in editor.Callbacks)
                {
                    callback.SaveGeneral(data);
                }
            }
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
