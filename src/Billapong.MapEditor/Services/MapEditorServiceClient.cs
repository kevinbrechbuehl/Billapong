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

        public async Task<IEnumerable<Map>> GetMapsAsync()
        {
            return await this.ExecuteAsync(() => this.Proxy.GetMaps());
        }

        public void DeleteMap(long mapId)
        {
            this.Execute(() => this.Proxy.DeleteMap(mapId));
        }

        public void SaveGeneral(GeneralMapData map)
        {
            this.Execute(() => this.Proxy.SaveGeneral(map));
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
