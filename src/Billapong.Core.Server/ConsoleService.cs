using System.Collections.Generic;
using Billapong.Contract.Data;
using Billapong.Contract.Service;

namespace Billapong.Core.Server
{
    public class ConsoleService : IConsoleService
    {
        public IEnumerable<Map> GetMaps()
        {
            yield return new Map { Id = 1, Name = "First map" };
            yield return new Map { Id = 2, Name = "Second map" };
        }
    }
}
