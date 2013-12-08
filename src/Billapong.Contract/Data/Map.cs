using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Contract.Data
{
    [DataContract(Name = "Map", Namespace = Globals.DataContractNamespaceName)]
    public class Map
    {
        [DataMember(Name = "Id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; }
    }
}
