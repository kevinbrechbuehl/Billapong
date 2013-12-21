using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.GameConsole.Service
{
    using Contract.Data.Editor;
    using Contract.Service;
    using Core.Client;

    public class EditorServiceClient : RichClientBase<IEditorService>, IEditorService
    {
        public IEnumerable<Map> GetMaps(bool includeUnplayable = false)
        {
            return base.Execute(() => base.Proxy.GetMaps());
        }
    }
}
