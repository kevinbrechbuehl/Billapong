using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
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

        public IList<IList<int>> GameWindows { get; private set; }

        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.Save);
            }
        }

        public DelegateCommand ToggleWindowButtonCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleWindowButton);
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
            var list = new List<int>();
            for (int i = 0; i < config.NumberOfCols; i++)
            {
                list.Add(i);
            }

            this.GameWindows = new List<IList<int>>();
            for (int i = 0; i < config.NumberOfRows; i++)
            {
                this.GameWindows.Add(list);
            }
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

        private void ToggleWindowButton()
        {
            MessageBox.Show("button clicked");
        }
    }
}
