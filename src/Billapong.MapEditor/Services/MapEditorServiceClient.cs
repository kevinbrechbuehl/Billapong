using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Services
{
    using System.Threading;

    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client;

    public class MapEditorServiceClient : CallbackClientBase<IMapEditorService, IMapEditorCallback>, IMapEditorService
    {
        public MapEditorServiceClient() : base(new MapEditorCallback())
        {
        }

        public MapEditorServiceClient(IMapEditorCallback callback) : base(callback)
        {
        }
        
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

        public async void DeleteMap(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.DeleteMap(mapId));
        }

        public async void UpdateName(long mapId, string name)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateName(mapId, name));
        }

        public async void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            await this.ExecuteAsync(() => this.Proxy.UpdateIsPlayable(mapId, isPlayable));
        }

        public async void AddWindow(long mapId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddWindow(mapId, coordX, coordY));
        }

        public async void RemoveWindow(long mapId, long windowId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveWindow(mapId, windowId));
        }

        public async void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            await this.ExecuteAsync(() => this.Proxy.AddHole(mapId, windowId, coordX, coordY));
        }

        public async void RemoveHole(long mapId, long windowId, long holeId)
        {
            await this.ExecuteAsync(() => this.Proxy.RemoveHole(mapId, windowId, holeId));
        }

        public void RegisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.RegisterCallback(mapId));
        }

        public void UnregisterCallback(long mapId)
        {
            this.Execute(() => this.Proxy.UnregisterCallback(mapId));
        }
    }
}
