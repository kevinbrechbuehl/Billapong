using Billapong.Contract.Data;
using Billapong.Contract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.Implementation
{
    public class AdministrationService : IAdministrationService
    {
        public IEnumerable<Map> GetMaps()
        {
            yield return new Map { Id = 1, Name = "First map" };
            yield return new Map { Id = 2, Name = "Second map" };
        }
    }
}
