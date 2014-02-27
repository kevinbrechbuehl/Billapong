namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Billapong.Core.Client.UI;
    using Billapong.GameConsole.Configuration;
    using Billapong.GameConsole.Models;
    using Billapong.GameConsole.Models.MapSelection;

    /// <summary>
    /// The base implementation of the window selection view model
    /// </summary>
    public class WindowSelectionViewModel
    {
        /// <summary>
        /// The map selection window size
        /// </summary>
        public const double MapSelectionWindowSize = 99;

        /// <summary>
        /// The hole diameter
        /// </summary>
        public const double HoleDiameter = MapSelectionWindowSize / GameConfiguration.GameGridSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSelectionViewModel" /> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public WindowSelectionViewModel(Map map)
        {
            this.Map = map;

            var windows = new MapSelectionWindow[GameConfiguration.MaxNumberOfWindowRows][];
           
            for (var row = 0; row < windows.Length; row++)
            {
                windows[row] = new MapSelectionWindow[GameConfiguration.MaxNumberOfWindowsPerRow];
                for (var col = 0; col < windows[row].Length; col++)
                {
                    var mapWindow = this.Map.Windows != null
                        ? this.Map.Windows.FirstOrDefault(item => item.X == col && item.Y == row)
                        : null;
                    windows[row][col] = new MapSelectionWindow(col, row, mapWindow, HoleDiameter);
                }
            }

            this.GameWindows = windows;
        }

        /// <summary>
        /// Occurs when the window selection changed.
        /// </summary>
        public event EventHandler WindowSelectionChanged = delegate { }; 

        /// <summary>
        /// Gets the game windows.
        /// </summary>
        /// <value>
        /// The game windows.
        /// </value>
        public MapSelectionWindow[][] GameWindows { get; private set; }

        /// <summary>
        /// Gets the map selection window clicked command.
        /// </summary>
        /// <value>
        /// The map selection window clicked command.
        /// </value>
        public DelegateCommand<MapSelectionWindow> MapSelectionWindowClickedCommand
        {
            get
            {
                return new DelegateCommand<MapSelectionWindow>(this.MapSelectionWindowClicked);
            }
        }

        /// <summary>
        /// Gets the selected windows.
        /// </summary>
        /// <value>
        /// The selected windows.
        /// </value>
        public IEnumerable<MapSelectionWindow> SelectedWindows
        {
            get
            {
                return from row in this.GameWindows from window in row where window != null && window.IsChecked select window;
            }
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        protected Map Map { get; private set; }

        /// <summary>
        /// Gets called when a window is clicked
        /// </summary>
        /// <param name="window">The window.</param>
        private void MapSelectionWindowClicked(MapSelectionWindow window)
        {
            if (!window.IsClickable) return;

            window.IsChecked = !window.IsChecked;
            this.WindowSelectionChanged(this, null);
        }
    }
}
