using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Services
{
    using System.ServiceModel;
    using System.Windows;

    using Billapong.Core.Client.Tracing;

    using Contract.Data.Map;
    using Contract.Service;
    using Core.Client.Helper;
    using Models.Events;

    [CallbackBehavior(UseSynchronizationContext = true)]
    public class MapEditorCallback : IMapEditorCallback
    {
        public event EventHandler<UpdateNameEventArgs> NameUpdated = delegate { };

        public event EventHandler<UpdateIsPlayableEventArgs> IsPlayableUpdated = delegate { };
        
        public event EventHandler<GameWindowEventArgs> WindowAdded = delegate { };

        public event EventHandler<GameWindowEventArgs> WindowRemoved = delegate { };

        public event EventHandler<GameHoleClickedEventArgs> HoleAdded = delegate { };

        public event EventHandler<GameHoleClickedEventArgs> HoleRemoved = delegate { };

        public void UpdateName(string name)
        {
            var args = new UpdateNameEventArgs(name);
            Tracer.Debug(string.Format("MapEditViewModel :: Name updated callback retrieved (Name={0})", args.Name));
            ThreadContext.InvokeOnUiThread(() => this.NameUpdated(this, args));
        }

        public void UpdateIsPlayable(bool isPlayable)
        {
            var args = new UpdateIsPlayableEventArgs(isPlayable);
            Tracer.Debug(string.Format("MapEditViewModel :: Is playable callback retrieved (Flag={0})", args.IsPlayable));
            ThreadContext.InvokeOnUiThread(() => this.IsPlayableUpdated(this, args));
        }

        public void AddWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            Tracer.Debug(string.Format("MapEditViewModel :: Window added callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.WindowAdded(this, args));
        }

        public void RemoveWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            Tracer.Debug(string.Format("MapEditViewModel :: Window removed callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.WindowRemoved(this, args));
        }

        public void AddHole(long windowId, int windowX, int windowY, long holeId, int holeX, int holeY)
        {
            var args = new GameHoleClickedEventArgs(windowId, windowX, windowY, holeId, holeX, holeY);
            Tracer.Debug(string.Format("MapEditViewModel :: Hole added callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.HoleAdded(this, args));
        }

        public void RemoveHole(long windowId, int windowX, int windowY, long holeId)
        {
            var args = new GameHoleClickedEventArgs(windowId, windowX, windowY, holeId);
            Tracer.Debug(string.Format("MapEditViewModel :: Hole removed callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.HoleRemoved(this, args));
        }
    }
}
