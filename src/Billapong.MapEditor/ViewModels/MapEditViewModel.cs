using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
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

        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.Save);
            }
        }

        public MapEditViewModel() : this (new Map())
        {
        }

        public MapEditViewModel(Map map)
        {
            this.callback = new MapEditorCallback();
            this.callback.GeneralDataSaved += this.GeneralDataSaved;
            this.proxy = new MapEditorServiceClient(this.callback);
            this.map = map;

            if (map.Id > 0)
            {
                this.proxy.RegisterCallback(map.Id);
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
    }
}
