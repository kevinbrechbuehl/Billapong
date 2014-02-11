namespace Billapong.Contract.Service
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.Map;

    /// <summary>
    /// Service contract for the map editor.
    /// </summary>
    [ServiceContract(Name = "MapEditor", CallbackContract = typeof(IMapEditorCallback), Namespace = Globals.ServiceContractNamespaceName)]
    public interface IMapEditorService
    {
        /// <summary>
        /// Gets all available maps on the server.
        /// </summary>
        /// <returns>List of available maps</returns>
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps();

        /// <summary>
        /// Gets the map configuration.
        /// </summary>
        /// <returns>Config with number of rows and cols</returns>
        [OperationContract(Name = "GetMapConfiguration")]
        MapConfiguration GetMapConfiguration();

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        [OperationContract(Name = "DeleteMap")]
        void DeleteMap(long mapId);

        /// <summary>
        /// Sets the name of the map.
        /// </summary>
        /// <param name="map">The map.</param>
        [OperationContract(Name = "SaveGeneral")]
        void SaveGeneral(GeneralMapData map);

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "AddWindow")]
        void AddWindow(long mapId, int coordX, int coordY);

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        [OperationContract(Name = "RemoveWindow")]
        void RemoveWindow(long mapId, long windowId);

        /// <summary>
        /// Registers the callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        [OperationContract(Name = "RegisterCallback")]
        void RegisterCallback(long mapId);

        /// <summary>
        /// Unregisters the callback.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        [OperationContract(Name = "UnregisterCallback")]
        void UnregisterCallback(long mapId); // todo (breck1): call this in the client
    }
}
