namespace Billapong.Core.Server.Map
{
    using System.Collections.Generic;
    using Contract.Service;

    public class MapEditor
    {
        public MapEditor()
        {
            this.Callbacks = new List<IMapEditorCallback>();
        }
        
        public IList<IMapEditorCallback> Callbacks { get; private set; }
    }
}
