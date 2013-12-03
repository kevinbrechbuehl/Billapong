using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Contract.Service
{
    [ServiceContract(Name = "BillapongAdministration", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IAdministrationService
    {
        [OperationContract(Name = "Test")]
        string Test(string name);
    }
}
