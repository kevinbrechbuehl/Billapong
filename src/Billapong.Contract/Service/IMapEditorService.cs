namespace Billapong.Contract.Service
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Data.Map;

    /// <summary>
    /// Service contract for the map editor.
    /// </summary>
    [ServiceContract(Name = "MapEditor", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IMapEditorService
    {
        /// <summary>
        /// Gets all available maps on the server.
        /// </summary>
        /// <returns>List of available maps</returns>
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps();
    }
}
