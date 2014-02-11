using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Services
{
    using System.ServiceModel;
    using System.Windows;
    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client.Helper;
    using Models.Events;

    [CallbackBehavior(UseSynchronizationContext = true)]
    public class MapEditorCallback : IMapEditorCallback
    {
        public event EventHandler<GeneralDataSavedEventArgs> GeneralDataSaved = delegate { };

        public event EventHandler<GameWindowEventArgs> WindowAdded = delegate { };

        public event EventHandler<GameWindowEventArgs> WindowRemoved = delegate { };
        
        public void SaveGeneral(GeneralMapData map)
        {
            var args = new GeneralDataSavedEventArgs(map.Id, map.Name);
            ThreadContext.InvokeOnUiThread(() => this.OnSavedGeneral(args));
        }

        public void AddWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            ThreadContext.InvokeOnUiThread(() => this.WindowAdded(this, args));
        }

        public void RemoveWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            ThreadContext.InvokeOnUiThread(() => this.WindowRemoved(this, args));
        }

        private void OnSavedGeneral(GeneralDataSavedEventArgs args)
        {
            this.GeneralDataSaved(this, args);
        }
    }
}
