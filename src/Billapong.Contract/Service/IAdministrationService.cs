using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Billapong.Contract.Data;

namespace Billapong.Contract.Service
{
    [ServiceContract(Name = "Administration", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IAdministrationService
    {
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps();
    }
}
