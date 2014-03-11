namespace Billapong.Contract.Service
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Billapong.Contract.Exceptions;
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
        /// Creates a new map.
        /// </summary>
        /// <returns>The newly created map</returns>
        [OperationContract(Name = "CreateMap")]
        Map CreateMap();

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        [OperationContract(Name = "DeleteMap")]
        void DeleteMap(long mapId);

        /// <summary>
        /// Updates the name.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="name">The name.</param>
        [OperationContract(Name = "UpdateName")]
        [FaultContract(typeof(CallbackNotValidException))]
        void UpdateName(long mapId, string name);

        /// <summary>
        /// Updates the is playable.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        [OperationContract(Name = "UpdateIsPlayable")]
        [FaultContract(typeof(CallbackNotValidException))]
        void UpdateIsPlayable(long mapId, bool isPlayable);

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "AddWindow")]
        [FaultContract(typeof(CallbackNotValidException))]
        void AddWindow(long mapId, int coordX, int coordY);

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        [OperationContract(Name = "RemoveWindow")]
        [FaultContract(typeof(CallbackNotValidException))]
        void RemoveWindow(long mapId, long windowId);

        /// <summary>
        /// Adds the hole.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "AddHole")]
        [FaultContract(typeof(CallbackNotValidException))]
        void AddHole(long mapId, long windowId, int coordX, int coordY);

        /// <summary>
        /// Removes the hole.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="holeId">The hole identifier.</param>
        [OperationContract(Name = "RemoveHole")]
        [FaultContract(typeof(CallbackNotValidException))]
        void RemoveHole(long mapId, long windowId, long holeId);

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
        void UnregisterCallback(long mapId);
    }
}
