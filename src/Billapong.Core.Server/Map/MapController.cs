namespace Billapong.Core.Server.Map
{
    using System;
    using Contract.Exceptions;
    using Contract.Service;
    using DataAccess.Model.Map;
    using DataAccess.Repository;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using DataAccess.UnitOfWork;

    /// <summary>
    /// The map controller
    /// </summary>
    public class MapController
    {
        /// <summary>
        /// The database writer lock object
        /// </summary>
        private static readonly object WriterLockObject = new object();

        /// <summary>
        /// The callback lock object
        /// </summary>
        private static readonly object CallbackLockObject = new object();

        /// <summary>
        /// All editors which are currently connected to the server
        /// </summary>
        private readonly IDictionary<long, MapEditor> editors = new Dictionary<long, MapEditor>();
        
        /// <summary>
        /// The map repository
        /// </summary>
        private readonly UnitOfWork UnitOfWork;

        /// <summary>
        /// The window repository
        /// </summary>
        private readonly IRepository<Window> windowRepository;

        /// <summary>
        /// The hole repository
        /// </summary>
        private readonly IRepository<Hole> holeRepository;

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
            this.UnitOfWork = new UnitOfWork();
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
            var maps = this.UnitOfWork.MapRepository.Get(includeProperties: "Windows, Windows.Holes");
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
            var maps = this.UnitOfWork.MapRepository.Get(filter: map => map.Id == id, includeProperties: "Windows, Windows.Holes");
            if (onlyPlayable)
            {
                maps = maps.Where(map => map.IsPlayable);
            }
            
            return maps.FirstOrDefault();
        }

        /// <summary>
        /// Deletes a map.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteMap(long id)
        {
            lock (WriterLockObject)
            {
                this.UnitOfWork.MapRepository.Remove(id);
                this.UnitOfWork.MapRepository.Save();
            }
        }

        /// <summary>
        /// Creates a new map.
        /// </summary>
        /// <returns>Newly created map</returns>
        public Map CreateMap()
        {
            lock (WriterLockObject)
            {
                return this.GetMap();
            }
        }

        /// <summary>
        /// Updates the name.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="name">The name.</param>
        public void UpdateName(long mapId, string name)
        {
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                map.Name = name;
                this.UnitOfWork.Save();
            }

            // send the callback
            Task.Run(() => this.SendUpdateNameCallback(mapId, name));
        }

        /// <summary>
        /// Updates the is playable flag.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        public void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            // todo (breck1): check if the map can be set to playable
            
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                map.IsPlayable = isPlayable;
                this.UnitOfWork.Save();
            }

            // send the callback
            Task.Run(() => this.SendUpdateIsPlayableCallback(mapId, isPlayable));
        }

        /// <summary>
        /// Adds a window to the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void AddWindow(long mapId, int coordX, int coordY)
        {
            var window = new Window {X = coordX, Y = coordY};
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                // do nothing if window already exists
                if (map.Windows.FirstOrDefault(innerWindow => innerWindow.X == coordX && innerWindow.Y == coordY) != null) return;

                map.Windows.Add(window);
                this.UnitOfWork.Save();
            }

             // send the callback
            Task.Run(() => this.SendAddWindowCallback(mapId, window.Id, coordX, coordY));
        }

        /// <summary>
        /// Removes a window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        public void RemoveWindow(long mapId, long windowId)
        {
            int coordX, coordY;
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                var window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                coordX = window.X;
                coordY = window.Y;
                this.UnitOfWork.WindowRepository.Remove(window.Id);
                this.UnitOfWork.Save();
            }

             // send the callback
            Task.Run(() => this.SendRemoveWindowCallback(mapId, windowId, coordX, coordY));
        }

        /// <summary>
        /// Adds a hole to a window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            var hole = new Hole {X = coordX, Y = coordY};
            Window window;
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                // do nothing if hole already exists
                if (window.Holes.FirstOrDefault(innerHole => innerHole.X == coordX && innerHole.Y == coordY) != null) return;

                window.Holes.Add(hole);
                this.UnitOfWork.Save();
            }

            // send the callback
            Task.Run(() => this.SendAddHoleCallback(mapId, windowId, window.X, window.Y, hole.Id, coordX, coordY));
        }

        /// <summary>
        /// Removes a hole.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="holeId">The hole identifier.</param>
        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            Window window;
            lock (WriterLockObject)
            {
                var map = this.GetMap(mapId);
                if (map == null) return;

                window = map.Windows.FirstOrDefault(gameWindow => gameWindow.Id == windowId);
                if (window == null) return;

                var hole = window.Holes.FirstOrDefault(gameHole => gameHole.Id == holeId);
                if (hole == null) return;

                this.UnitOfWork.HoleRepository.Remove(hole.Id);
                this.UnitOfWork.Save();
            }

            // send the callback
            Task.Run(() => this.SendRemoveHoleCallback(mapId, windowId, window.X, window.Y, holeId));
        }

        /// <summary>
        /// Registers a callback to a specific map.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="callback">The callback.</param>
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

        /// <summary>
        /// Unregisters a callback form a specific map.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="callback">The callback.</param>
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

        /// <summary>
        /// Sends the update name callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="name">The name.</param>
        private void SendUpdateNameCallback(long mapId, string name)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.UpdateName(name);
            }
        }

        /// <summary>
        /// Sends the update is playable callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        private void SendUpdateIsPlayableCallback(long mapId, bool isPlayable)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.UpdateIsPlayable(isPlayable);
            }
        }

        /// <summary>
        /// Sends the add window callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x coords.</param>
        /// <param name="coordY">The coord y coords.</param>
        private void SendAddWindowCallback(long mapId, long windowId, int coordX, int coordY)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.AddWindow(windowId, coordX, coordY);
            }
        }

        /// <summary>
        /// Sends the remove window callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x coords.</param>
        /// <param name="coordY">The coord y coords.</param>
        private void SendRemoveWindowCallback(long mapId, long windowId, int coordX, int coordY)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.RemoveWindow(windowId, coordX, coordY);
            }
        }

        /// <summary>
        /// Sends the add hole callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x coords.</param>
        /// <param name="windowY">The window y coords.</param>
        /// <param name="holeId">The hole identifier.</param>
        /// <param name="holeX">The hole x coords.</param>
        /// <param name="holeY">The hole y coords.</param>
        private void SendAddHoleCallback(long mapId, long windowId, int windowX, int windowY, long holeId, int holeX, int holeY)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.AddHole(windowId, windowX, windowY, holeId, holeX, holeY);
            }
        }

        /// <summary>
        /// Sends the remove hole callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x coords.</param>
        /// <param name="windowY">The window y coords.</param>
        /// <param name="holeId">The hole identifier.</param>
        private void SendRemoveHoleCallback(long mapId, long windowId, int windowX, int windowY, long holeId)
        {
            foreach (var callback in this.GetMapEditorCallbacks(mapId))
            {
                callback.RemoveHole(windowId, windowX, windowY, holeId);
            }
        }

        /// <summary>
        /// Gets the map editor callbacks.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>Callbacks registered for a specific map</returns>
        private IEnumerable<IMapEditorCallback> GetMapEditorCallbacks(long mapId)
        {
            var callbacks = new List<IMapEditorCallback>();
            lock (CallbackLockObject)
            {
                var editor = this.editors.ContainsKey(mapId) ? this.editors[mapId] : null;
                if (editor == null) return Enumerable.Empty<IMapEditorCallback>();

                foreach (var callback in editor.Callbacks.ToList())
                {
                    if (((ICommunicationObject) callback).State != CommunicationState.Opened)
                    {
                        editor.Callbacks.Remove(callback);
                        continue;
                    }

                    callbacks.Add(callback);
                }
            }

            return callbacks;
        }

        /// <summary>
        /// Gets a map from the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Map from the database or a new map if id was 0</returns>
        private Map GetMap(long id = 0)
        {
            if (id > 0)
            {
                var databaseMap = this.UnitOfWork.MapRepository.GetById(id);
                if (databaseMap == null)
                {
                    throw new FaultException<MapNotFoundException>(new MapNotFoundException(id), "Map not found");
                }

                return databaseMap;
            }

            var map = new Map
            {
                Name = "<Unnamed>",
                IsPlayable = false
            };

            this.UnitOfWork.MapRepository.Add(map);
            this.UnitOfWork.Save();
            return map;
        }
    }
}
