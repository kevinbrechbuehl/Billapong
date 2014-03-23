namespace Billapong.MapEditor.Services
{
    using System;
    using System.ServiceModel;
    using Billapong.Core.Client.Tracing;
    using Contract.Service;
    using Core.Client.Helper;
    using Models.Events;

    /// <summary>
    /// Callback for the map editor.
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class MapEditorCallback : IMapEditorCallback
    {
        /// <summary>
        /// Occurs when name was updated.
        /// </summary>
        public event EventHandler<UpdateNameEventArgs> NameUpdated = delegate { };

        /// <summary>
        /// Occurs when is playable was updated.
        /// </summary>
        public event EventHandler<UpdateIsPlayableEventArgs> IsPlayableUpdated = delegate { };

        /// <summary>
        /// Occurs when window is added.
        /// </summary>
        public event EventHandler<GameWindowEventArgs> WindowAdded = delegate { };

        /// <summary>
        /// Occurs when window is removed.
        /// </summary>
        public event EventHandler<GameWindowEventArgs> WindowRemoved = delegate { };

        /// <summary>
        /// Occurs when hole is added.
        /// </summary>
        public event EventHandler<GameHoleClickedEventArgs> HoleAdded = delegate { };

        /// <summary>
        /// Occurs when hole is removed.
        /// </summary>
        public event EventHandler<GameHoleClickedEventArgs> HoleRemoved = delegate { };

        /// <summary>
        /// Updates the name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void UpdateName(string name)
        {
            var args = new UpdateNameEventArgs(name);
            Tracer.Debug(string.Format("MapEditViewModel :: Name updated callback retrieved (Name={0})", args.Name));
            ThreadContext.InvokeOnUiThread(() => this.NameUpdated(this, args));
        }

        /// <summary>
        /// Updates the is playable.
        /// </summary>
        /// <param name="isPlayable">if set to <c>true</c> the map is playable.</param>
        public void UpdateIsPlayable(bool isPlayable)
        {
            var args = new UpdateIsPlayableEventArgs(isPlayable);
            Tracer.Debug(string.Format("MapEditViewModel :: Is playable callback retrieved (Flag={0})", args.IsPlayable));
            ThreadContext.InvokeOnUiThread(() => this.IsPlayableUpdated(this, args));
        }

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void AddWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            Tracer.Debug(string.Format("MapEditViewModel :: Window added callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.WindowAdded(this, args));
        }

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="coordX">The coord x.</param>
        /// <param name="coordY">The coord y.</param>
        public void RemoveWindow(long windowId, int coordX, int coordY)
        {
            var args = new GameWindowEventArgs(windowId, coordX, coordY);
            Tracer.Debug(string.Format("MapEditViewModel :: Window removed callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.WindowRemoved(this, args));
        }

        /// <summary>
        /// Adds the hole.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x.</param>
        /// <param name="windowY">The window y.</param>
        /// <param name="holeId">The hole identifier.</param>
        /// <param name="holeX">The hole x.</param>
        /// <param name="holeY">The hole y.</param>
        public void AddHole(long windowId, int windowX, int windowY, long holeId, int holeX, int holeY)
        {
            var args = new GameHoleClickedEventArgs(windowId, windowX, windowY, holeId, holeX, holeY);
            Tracer.Debug(string.Format("MapEditViewModel :: Hole added callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.HoleAdded(this, args));
        }

        /// <summary>
        /// Removes the hole.
        /// </summary>
        /// <param name="windowId">The window identifier.</param>
        /// <param name="windowX">The window x.</param>
        /// <param name="windowY">The window y.</param>
        /// <param name="holeId">The hole identifier.</param>
        public void RemoveHole(long windowId, int windowX, int windowY, long holeId)
        {
            var args = new GameHoleClickedEventArgs(windowId, windowX, windowY, holeId);
            Tracer.Debug(string.Format("MapEditViewModel :: Hole removed callback retrieved ({0})", args));
            ThreadContext.InvokeOnUiThread(() => this.HoleRemoved(this, args));
        }
    }
}
