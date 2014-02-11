namespace Billapong.Contract.Service
{
    using System.ServiceModel;
    using Data.Map;

    /// <summary>
    /// Callback contract for the map editor.
    /// </summary>
    public interface IMapEditorCallback
    {
        /// <summary>
        /// Saves the general data of a map.
        /// </summary>
        /// <param name="map">The map.</param>
        [OperationContract(Name = "SaveGeneral")]
        void SaveGeneral(GeneralMapData map);

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "AddWindow")]
        void AddWindow(long windowId, int coordX, int coordY);

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        [OperationContract(Name = "RemoveWindow")]
        void RemoveWindow(long windowId, int coordX, int coordY);
    }
}
