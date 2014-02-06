namespace Billapong.Core.Server.Services
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using Contract.Data.Map;
    using Contract.Service;
    using Converter.Map;
    using Map;

    /// <summary>
    /// Service implementation for the map editor.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MapEditorService : IMapEditorService
    {
        /// <summary>
        /// Gets all available maps on the server.
        /// </summary>
        /// <returns>
        /// List of available maps
        /// </returns>
        public IEnumerable<Map> GetMaps()
        {
            return MapController.Current.GetMaps().Select(map => map.ToContract()).ToList();
        }

        /// <summary>
        /// Gets the map configuration.
        /// </summary>
        /// <returns>
        /// Config with number of rows and cols
        /// </returns>
        public MapConfiguration GetMapConfiguration()
        {
            int numberOfWindowRows;
            if (!int.TryParse(ConfigurationManager.AppSettings["MapEditor.NumberOfWindowRows"], out numberOfWindowRows))
            {
                numberOfWindowRows = 3;
            }

            int numberOfWindowCols;
            if (!int.TryParse(ConfigurationManager.AppSettings["MapEditor.NumberOfWindowCols"], out numberOfWindowCols))
            {
                numberOfWindowCols = 4;
            }

            int numberOfHoleRows;
            if (!int.TryParse(ConfigurationManager.AppSettings["MapEditor.numberOfHoleRows"], out numberOfHoleRows))
            {
                numberOfHoleRows = 15;
            }

            int numberOfHoleCols;
            if (!int.TryParse(ConfigurationManager.AppSettings["MapEditor.numberOfHoleCols"], out numberOfHoleCols))
            {
                numberOfHoleCols = 4;
            }

            return new MapConfiguration()
            {
                NumberOfWindowCols = numberOfWindowCols,
                NumberOfWindowRows = numberOfWindowRows,
                NumberOfHoleCols = numberOfHoleCols,
                NumberOfHoleRows = numberOfHoleRows
            };
        }

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void DeleteMap(long mapId)
        {
            MapController.Current.DeleteMap(mapId);
        }

        /// <summary>
        /// Sets the name of the map.
        /// </summary>
        /// <param name="map"></param>
        public void SaveGeneral(GeneralMapData map)
        {
            MapController.Current.SaveGeneral(map.Id, map.Name, this.GetCallback());
        }

        public void RegisterCallback(long mapId)
        {
            MapController.Current.RegisterCallback(mapId, this.GetCallback());
        }

        public void UnregisterCallback(long mapId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <returns>Callback channel for the current context</returns>
        private IMapEditorCallback GetCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IMapEditorCallback>();
        }
    }
}
