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

    public class MapEditorServiceClient : RichClientBase<IMapEditorService>, IMapEditorService
    {
        public IEnumerable<Map> GetMaps()
        {
            return this.Execute(() => this.Proxy.GetMaps());
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

        public async void DeleteMapAsync(long mapId)
        {
            await this.ExecuteAsync(() => this.Proxy.DeleteMap(mapId));
        }
    }
}
