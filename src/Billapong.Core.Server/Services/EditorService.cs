namespace Billapong.Core.Server.Services
{
    using Contract.Data.Editor;
    using Contract.Service;
    using Converter.Editor;
    using Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// The editor service implementation.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EditorService : IEditorService
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <param name="includeUnplayable">if set to <c>true</c> unplayable maps will also returned.</param>
        /// <returns>
        /// Available maps in the database
        /// </returns>
        public IEnumerable<Map> GetMaps(bool includeUnplayable = false)
        {
            return MapController.Current.GetMaps().Select(map => map.ToContract()).ToList();
        }
    }
}
