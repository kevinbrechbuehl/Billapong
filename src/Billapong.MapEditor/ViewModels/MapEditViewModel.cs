using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
    using System.Globalization;
    using System.Windows;
    using Contract.Service;
    using Core.Client.UI;
    using Converter;
    using Models;
    using Models.Events;
    using Services;

    public class MapEditViewModel : ViewModelBase
    {
        private readonly Map map;

        private readonly MapEditorServiceClient proxy;

        private readonly MapEditorCallback callback;

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

        public Models.Window[][] GameWindows { get; private set; }

        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.Save);
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
            for (var i = 0; i < windows.Length; i++)
            {
                windows[i] = new Models.Window[config.WindowCols];
                for (var j=0; j<windows[i].Length; j++)
                {
                    windows[i][j] = new Models.Window(j, i);
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

        private void ToggleWindow(Models.Window window)
        {
            MessageBox.Show(string.Format("window clicked - x: {0}, y: {1}", window.X, window.Y));
        }
    }
}
