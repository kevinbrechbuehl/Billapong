using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
    using System.Globalization;
    using System.ServiceModel.Channels;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Input;
    using System.Windows.Media;
    using Contract.Service;
    using Core.Client.UI;
    using Converter;
    using Models;
    using Models.Events;
    using Services;

    public class MapEditViewModel : ViewModelBase
    {
        public const double GameWindowSize = 200;
        
        private readonly Map map;

        private readonly MapEditorServiceClient proxy;

        private readonly MapEditorCallback callback;

        private readonly double HoleDiameter;

        public string MapName
        {
            get
            {
                return this.map.Name;
            }

            set
            {
                this.map.Name = value;
                OnPropertyChanged();
            }
        }

        private bool isWindowHolesSelectionMode;

        public Models.Window[][] GameWindows { get; private set; }

        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.Save);
            }
        }

        public DelegateCommand ToggleSelectionModeCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleSelectionMode);
            }
        }

        public DelegateCommand<Models.Window> ToggleWindowCommand
        {
            get
            {
                return new DelegateCommand<Models.Window>(this.ToggleWindow);
            }
        }

        public MapEditViewModel() : this (new Map())
        {
        }

        public MapEditViewModel(Map map)
        {
            // initialize
            this.callback = new MapEditorCallback();
            this.callback.GeneralDataSaved += this.GeneralDataSaved;
            this.proxy = new MapEditorServiceClient(this.callback);
            this.map = map;

            // register the callback
            if (map.Id > 0)
            {
                this.proxy.RegisterCallback(map.Id);
            }

            // get the maps config and display it
            var config = this.proxy.GetMapConfiguration();
            var windows = new Models.Window[config.WindowRows][];
            this.HoleDiameter = GameWindowSize/config.HoleGrid;

            for (var i = 0; i < windows.Length; i++)
            {
                windows[i] = new Models.Window[config.WindowCols];
                for (var j=0; j<windows[i].Length; j++)
                {
                    windows[i][j] = new Models.Window(j, i, this.map.Windows.FirstOrDefault(item => item.X == i && item.Y == j), this.HoleDiameter);
                }
            }

            this.GameWindows = windows;
        }

        public void GeneralDataSaved(object sender, GeneralDataSavedEventArgs args)
        {
            this.map.Id = args.Id;
            this.MapName = args.Name;
        }

        private void Save()
        {
            this.proxy.SaveGeneral(this.map.ToGeneralMapData());
        }

        private void ToggleSelectionMode()
        {
            this.isWindowHolesSelectionMode = !isWindowHolesSelectionMode;
            Mouse.OverrideCursor = this.isWindowHolesSelectionMode ? Cursors.Cross : null;
        }

        private void ToggleWindow(Models.Window window)
        {
            if (this.isWindowHolesSelectionMode)
            {
                window.Holes.Add(new Hole {X = 3,  Y = 3, Diameter = this.HoleDiameter });
            }
            else
            {
                window.IsChecked = !window.IsChecked;
            }
        }
    }
}
