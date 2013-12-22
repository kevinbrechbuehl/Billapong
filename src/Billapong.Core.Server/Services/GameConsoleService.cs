namespace Billapong.Core.Server.Services
{
    using Contract.Data.Map;
    using Contract.Service;
    using Converter.Map;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Map;

    /// <summary>
    /// The game console service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameConsoleService : IGameConsoleService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <returns>
        /// Playable maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return MapController.Current.GetMaps(true).Select(map => map.ToContract()).ToList();
        }
    }
}
