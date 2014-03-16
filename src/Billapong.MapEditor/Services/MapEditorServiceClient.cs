using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Services
{
    using System.Threading;
    using Billapong.Core.Client.Authentication;
    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client;

    public class MapEditorServiceClient : CallbackClientBase<IMapEditorService, IMapEditorCallback>, IMapEditorService
    {
        public MapEditorServiceClient(Guid sessionId)
            : this(new MapEditorCallback(), sessionId)
        {
        }

        public MapEditorServiceClient(IMapEditorCallback callback,  Guid sessionId)
            : base(callback, new AuthenticationProvider(sessionId))
        {
        }

        #region Synchronous
        
        public IEnumerable<Map> GetMaps()
        {
            return this.Execute(() => this.Proxy.GetMaps());
        }

        public MapConfiguration GetMapConfiguration()
        {
            return this.Execute(() => this.Proxy.GetMapConfiguration());
        }

        public Map CreateMap()
        {
            return this.Execute(() => this.Proxy.CreateMap());
        }

        public void DeleteMap(long mapId)
        {
            this.Execute(() => this.Proxy.DeleteMap(mapId));
        }

        public void RegisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.RegisterCallback(mapId));
        }

        public void UnregisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.UnregisterCallback(mapId));
        }

        public void UpdateName(long mapId, string name)
        {
            this.Execute(() => this.Proxy.UpdateName(mapId, name));
        }

        public void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            this.Execute(() => this.Proxy.UpdateIsPlayable(mapId, isPlayable));
        }

        public void AddWindow(long mapId, int coordX, int coordY)
        {
            this.Execute(() => this.Proxy.AddWindow(mapId, coordX, coordY));
        }

        public void RemoveWindow(long mapId, long windowId)
        {
            this.Execute(() => this.Proxy.RemoveWindow(mapId, windowId));
        }

        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            this.Execute(() => this.Proxy.AddHole(mapId, windowId, coordX, coordY));
        }

        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            this.Execute(() => this.Proxy.RemoveHole(mapId, windowId, holeId));
        }

        #endregion

        #region Asynchronous

        public async Task<IEnumerable<Map>> GetMapsAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMaps());
        }

        public async Task<MapConfiguration> GetMapConfigurationAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMapConfiguration());
        }

        public async Task<Map> CreateMapAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.CreateMap());
        }

        public async Task DeleteMapAsync(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.DeleteMap(mapId));
        }

        public async Task UpdateNameAsync(long mapId, string name)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateName(mapId, name));
        }

        public async Task UpdateIsPlayableAsync(long mapId, bool isPlayable)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateIsPlayable(mapId, isPlayable));
        }

        public async Task AddWindowAsync(long mapId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddWindow(mapId, coordX, coordY));
        }

        public async Task RemoveWindowAsync(long mapId, long windowId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveWindow(mapId, windowId));
        }

        public async Task AddHoleAsync(long mapId, long windowId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddHole(mapId, windowId, coordX, coordY));
        }

        public async Task RemoveHoleAsync(long mapId, long windowId, long holeId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveHole(mapId, windowId, holeId));
        }

        #endregion
    }
}
