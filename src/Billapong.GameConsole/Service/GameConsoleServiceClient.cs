using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.GameConsole.Service
{
    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client;

    public class GameConsoleServiceClient : RichClientBase<IGameConsoleService>, IGameConsoleService
    {
        public IEnumerable<Map> GetMaps()
        {
            return base.Execute(() => base.Proxy.GetMaps());
        }
    }
}
