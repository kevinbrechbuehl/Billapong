namespace Billapong.Contract.Service
{
    using Data.Editor;
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// The editor service contract
    /// </summary>
    [ServiceContract(Name = "Editor", Namespace = Globals.ServiceContractNamespaceName)]
    public interface IEditorService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <param name="includeUnplayable">if set to <c>true</c> unplayable maps will also returned.</param>
        /// <returns>Available maps in the database</returns>
        [OperationContract(Name = "GetMaps")]
        IEnumerable<Map> GetMaps(bool includeUnplayable = false);
    }
}
