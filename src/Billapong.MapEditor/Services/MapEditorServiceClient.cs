using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Services
{
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

        public void DeleteMap(long mapId)
        {
            this.Execute(() => this.Proxy.DeleteMap(mapId));
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
            // todo (breck1): call this on window close
            this.Execute(() => this.Proxy.UnregisterCallback(mapId));
        }

        public async void DeleteMapAsync(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.DeleteMap(mapId));
        }

        public async void RegisterCallbackAsync(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.RegisterCallback(mapId));
        }
    }
}
