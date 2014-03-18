namespace Billapong.MapEditor.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Billapong.Core.Client.Authentication;
    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client;

    /// <summary>
    /// Map editor service proxy.
    /// </summary>
    public class MapEditorServiceClient : CallbackClientBase<IMapEditorService, IMapEditorCallback>, IMapEditorService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditorServiceClient"/> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public MapEditorServiceClient(Guid sessionId)
            : this(new MapEditorCallback(), sessionId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditorServiceClient"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="sessionId">The session identifier.</param>
        public MapEditorServiceClient(IMapEditorCallback callback,  Guid sessionId)
            : base(callback, new AuthenticationProvider(sessionId))
        {
        }

        #region Synchronous

        /// <summary>
        /// Gets all available maps on the server.
        /// </summary>
        /// <returns>
        /// List of available maps
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return this.Execute(() => this.Proxy.GetMaps());
        }

        /// <summary>
        /// Gets the map configuration.
        /// </summary>
        /// <returns>
        /// Config with number of rows and cols
        /// </returns>
        public MapConfiguration GetMapConfiguration()
        {
            return this.Execute(() => this.Proxy.GetMapConfiguration());
        }

        /// <summary>
        /// Creates a new map.
        /// </summary>
        /// <returns>
        /// The newly created map
        /// </returns>
        public Map CreateMap()
        {
            return this.Execute(() => this.Proxy.CreateMap());
        }

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void DeleteMap(long mapId)
        {
            this.Execute(() => this.Proxy.DeleteMap(mapId));
        }

        /// <summary>
        /// Registers the callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void RegisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.RegisterCallback(mapId));
        }

        /// <summary>
        /// Unregisters the callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void UnregisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.UnregisterCallback(mapId));
        }

        /// <summary>
        /// Updates the name.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="name">The name.</param>
        public void UpdateName(long mapId, string name)
        {
            this.Execute(() => this.Proxy.UpdateName(mapId, name));
        }

        /// <summary>
        /// Updates the is playable.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        public void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            this.Execute(() => this.Proxy.UpdateIsPlayable(mapId, isPlayable));
        }

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void AddWindow(long mapId, int coordX, int coordY)
        {
            this.Execute(() => this.Proxy.AddWindow(mapId, coordX, coordY));
        }

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        public void RemoveWindow(long mapId, long windowId)
        {
            this.Execute(() => this.Proxy.RemoveWindow(mapId, windowId));
        }

        /// <summary>
        /// Adds the hole.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            this.Execute(() => this.Proxy.AddHole(mapId, windowId, coordX, coordY));
        }

        /// <summary>
        /// Removes the hole.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="holeId">The hole identifier.</param>
        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            this.Execute(() => this.Proxy.RemoveHole(mapId, windowId, holeId));
        }

        #endregion

        #region Asynchronous

        /// <summary>
        /// Gets all available maps on the server asynchronous.
        /// </summary>
        /// <returns>
        /// List of available maps
        /// </returns>
        public async Task<IEnumerable<Map>> GetMapsAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMaps());
        }

        /// <summary>
        /// Gets the map configuration asynchronous.
        /// </summary>
        /// <returns>
        /// Config with number of rows and cols
        /// </returns>
        public async Task<MapConfiguration> GetMapConfigurationAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMapConfiguration());
        }

        /// <summary>
        /// Creates a new map asynchronous.
        /// </summary>
        /// <returns>
        /// The newly created map
        /// </returns>
        public async Task<Map> CreateMapAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.CreateMap());
        }

        /// <summary>
        /// Deletes the map asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>Async task</returns>
        public async Task DeleteMapAsync(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.DeleteMap(mapId));
        }

        /// <summary>
        /// Updates the name asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>Async task</returns>
        public async Task UpdateNameAsync(long mapId, string name)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateName(mapId, name));
        }

        /// <summary>
        /// Updates the is playable asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        /// <returns>Async task</returns>
        public async Task UpdateIsPlayableAsync(long mapId, bool isPlayable)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateIsPlayable(mapId, isPlayable));
        }

        /// <summary>
        /// Adds the window asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        /// <returns>Async task</returns>
        public async Task AddWindowAsync(long mapId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddWindow(mapId, coordX, coordY));
        }

        /// <summary>
        /// Removes the window asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <returns>Async task</returns>
        public async Task RemoveWindowAsync(long mapId, long windowId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveWindow(mapId, windowId));
        }

        /// <summary>
        /// Adds the hole asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        /// <returns>Async task</returns>
        public async Task AddHoleAsync(long mapId, long windowId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddHole(mapId, windowId, coordX, coordY));
        }

        /// <summary>
        /// Removes the hole asynchronous.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="holeId">The hole identifier.</param>
        /// <returns>Async task</returns>
        public async Task RemoveHoleAsync(long mapId, long windowId, long holeId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveHole(mapId, windowId, holeId));
        }

        #endregion
    }
}
