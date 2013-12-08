using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Billapong.Contract.Data;

namespace Billapong.Contract.Service
{
    [ServiceContract(Name = "Console", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IConsoleService
    {
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps();
    }
}
