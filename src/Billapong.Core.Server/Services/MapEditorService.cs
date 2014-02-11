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
            int windowRows;
            if (!int.TryParse(ConfigurationManager.AppSettings["Map.WindowRows"], out windowRows))
            {
                windowRows = 3;
            }

            int windowCols;
            if (!int.TryParse(ConfigurationManager.AppSettings["Map.WindowCols"], out windowCols))
            {
                windowCols = 4;
            }

            int holeGrid;
            if (!int.TryParse(ConfigurationManager.AppSettings["Map.HoleGrid"], out holeGrid))
            {
                holeGrid = 15;
            }

            return new MapConfiguration()
            {
                WindowCols = windowCols,
                WindowRows = windowRows,
                HoleGrid = holeGrid
            };
        }

        public Map CreateMap()
        {
            return MapController.Current.CreateMap().ToContract();
        }

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void DeleteMap(long mapId)
        {
            MapController.Current.DeleteMap(mapId);
        }

        public void UpdateName(long mapId, string name)
        {
            MapController.Current.UpdateName(mapId, name);
        }

        public void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            MapController.Current.UpdateIsPlayable(mapId, isPlayable);
        }

        public void AddWindow(long mapId, int coordX, int coordY)
        {
            MapController.Current.AddWindow(mapId, coordX, coordY);
        }

        public void RemoveWindow(long mapId, long windowId)
        {
            MapController.Current.RemoveWindow(mapId, windowId);
        }

        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            MapController.Current.AddHole(mapId, windowId, coordX, coordY);
        }

        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            MapController.Current.RemoveHole(mapId, windowId, holeId);
        }

        public void RegisterCallback(long mapId)
        {
            MapController.Current.RegisterCallback(mapId, this.GetCallback());
        }

        public void UnregisterCallback(long mapId)
        {
            MapController.Current.UnregisterCallback(mapId, this.GetCallback());
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
