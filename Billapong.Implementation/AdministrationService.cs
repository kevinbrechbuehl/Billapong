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
        public string Test(string name)
        {
            return string.Format("Hello {0}", name);
        }
    }
}
