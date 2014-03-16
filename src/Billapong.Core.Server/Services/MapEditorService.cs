namespace Billapong.Core.Server.Services
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Server.Tracing;

    using Contract.Data.Map;
    using Contract.Service;
    using Converter.Map;
    using Map;

    /// <summary>
    /// Service implementation for the map editor.
    /// </summary>
    [Authentication.Authentication(Role.Editor)]
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
            Tracer.Debug("MapEditorService :: GetMaps() called");
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
            Tracer.Debug("MapEditorService :: GetMapConfiguration() called");
            
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
            Tracer.Debug("MapEditorService :: CreateMap() called");
            return MapController.Current.CreateMap().ToContract();
        }

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void DeleteMap(long mapId)
        {
            Tracer.Debug(string.Format("MapEditorService :: DeleteMap called with mapId={0}", mapId));
            MapController.Current.DeleteMap(mapId);
        }

        public void UpdateName(long mapId, string name)
        {
            Tracer.Debug(string.Format("MapEditorService :: UpdateName() called with mapId={0}, name={1}", mapId, name));
            this.VerifyCallback(mapId);
            MapController.Current.UpdateName(mapId, name);
        }

        public void UpdateIsPlayable(long mapId, bool isPlayable)
        {
            Tracer.Debug(string.Format("MapEditorService :: UpdateIsPlayable() called with mapId={0}, isPlayable={1}", mapId, isPlayable));
            this.VerifyCallback(mapId);
            MapController.Current.UpdateIsPlayable(mapId, isPlayable);
        }

        public void AddWindow(long mapId, int coordX, int coordY)
        {
            Tracer.Debug(string.Format("MapEditorService :: AddWindow() called with mapId={0}, coordX={1}, coordY={2}", mapId, coordX, coordY));
            this.VerifyCallback(mapId);
            MapController.Current.AddWindow(mapId, coordX, coordY);
        }

        public void RemoveWindow(long mapId, long windowId)
        {
            Tracer.Debug(string.Format("MapEditorService :: RemoveWindow() called with mapId={0}, windowId={1}", mapId, windowId));
            this.VerifyCallback(mapId);
            MapController.Current.RemoveWindow(mapId, windowId);
        }

        public void AddHole(long mapId, long windowId, int coordX, int coordY)
        {
            Tracer.Debug(string.Format("MapEditorService :: AddHole() called with mapId={0}, windowId={1} coordX={2}, coordY={3}", mapId, windowId, coordX, coordY));
            this.VerifyCallback(mapId);
            MapController.Current.AddHole(mapId, windowId, coordX, coordY);
        }

        public void RemoveHole(long mapId, long windowId, long holeId)
        {
            Tracer.Debug(string.Format("MapEditorService :: RemoveHole() called with mapId={0}, windowId={1}, holeId={2}", mapId, windowId, holeId));
            this.VerifyCallback(mapId);
            MapController.Current.RemoveHole(mapId, windowId, holeId);
        }

        public void RegisterCallback(long mapId)
        {
            Tracer.Debug(string.Format("MapEditorService :: RegisterCallback() called with mapId={0}", mapId));
            MapController.Current.RegisterCallback(mapId, this.GetCallback());
        }

        public void UnregisterCallback(long mapId)
        {
            Tracer.Debug(string.Format("MapEditorService :: UnregisterCallback() called with mapId={0}", mapId));
            MapController.Current.UnregisterCallback(mapId, this.GetCallback());
        }

        private void VerifyCallback(long mapId)
        {
            if (!MapController.Current.IsCallbackRegistered(mapId, this.GetCallback()))
            {
                throw new FaultException<CallbackNotValidException>(new CallbackNotValidException(), "Callback not valid");
            }
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
