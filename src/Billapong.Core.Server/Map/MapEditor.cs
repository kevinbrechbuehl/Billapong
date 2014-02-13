namespace Billapong.Core.Server.Map
{
    using System.Collections.Generic;
    using Contract.Service;

    /// <summary>
    /// Class for storing all callbacks to a specific map.
    /// </summary>
    public class MapEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditor"/> class.
        /// </summary>
        public MapEditor()
        {
            this.Callbacks = new List<IMapEditorCallback>();
        }

        /// <summary>
        /// Gets the callbacks.
        /// </summary>
        /// <value>
        /// The callbacks.
        /// </value>
        public IList<IMapEditorCallback> Callbacks { get; private set; }
    }
}
