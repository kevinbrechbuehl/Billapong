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
    }
}
