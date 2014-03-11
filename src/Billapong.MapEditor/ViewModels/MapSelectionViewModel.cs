namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Billapong.Core.Client.Exceptions;
    using Billapong.Core.Client.Tracing;
    using Billapong.MapEditor.Properties;

    using Converter;
    using Core.Client.UI;
    using Models;
    using Services;

    public class MapSelectionViewModel : ViewModelBase
    {
        /// <summary>
        /// The is data loading
        /// </summary>
        private bool isDataLoading;

        private MapEditorServiceClient proxy;

        /// <summary>
        /// Gets or sets a value indicating whether the view data is loading.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the view data is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataLoading
        {
            get
            {
                return this.isDataLoading;
            }

            set
            {
                this.isDataLoading = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Map> Maps { get; private set; }

        public DelegateCommand CreateNewMapCommand
        {
            get
            {
                return new DelegateCommand(this.CreateNewMap);
            }
        }

        public DelegateCommand RefreshMapsCommand
        {
            get
            {
                return new DelegateCommand(this.RefreshMaps);
            }
        }

        public DelegateCommand<Map> DeleteMapCommand
        {
            get
            {
                return new DelegateCommand<Map>(this.DeleteMap);
            }
        }

        public DelegateCommand<Map> EditMapCommand
        {
            get
            {
                return new DelegateCommand<Map>(this.EditMap);
            }
        }

        public MapSelectionViewModel()
        {
            this.Initialize();
        }

        private async void Initialize()
        {
            this.proxy = new MapEditorServiceClient();
            this.Maps = new ObservableCollection<Map>();
            await this.LoadMaps();
        }

        private async Task LoadMaps()
        {
            await Tracer.Info("MapSelectionViewModel :: refresh maps");

            this.IsDataLoading = true;

            try
            {
                var maps = await this.proxy.GetMapsAsync();
                this.Maps.Clear();
                foreach (var map in maps)
                {
                    this.Maps.Add(map.ToEntity());
                }
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }

            this.IsDataLoading = false;
        }

        private async Task DeleteMap(long id)
        {
            await Tracer.Info(string.Format("MapSelectionViewModel :: Delete map with id '{0}'", id));

            try
            {
                await this.proxy.DeleteMapAsync(id);
                await this.LoadMaps();
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }
        }

        private async void DeleteMap(Map map)
        {
            if (MessageBox.Show(string.Format(Resources.DeleteMapQuestion, ((Map)map).Name), Resources.ConfirmationTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await this.DeleteMap(map.Id);
            }
        }

        private async void EditMap(Map map)
        {
            // refresh maps for getting correct versions
            await this.LoadMaps();

            var mapToEdit = this.Maps.FirstOrDefault(m => m.Id == map.Id);
            if (mapToEdit == null)
            {
                MessageBox.Show(Resources.MapNotExist, Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.WindowManager.Open(new MapEditViewModel(mapToEdit));
        }

        private async void CreateNewMap()
        {
            await Tracer.Info("MapSelectionViewModel :: Create new map");

            try
            {
                var map = await this.proxy.CreateMapAsync();
                var entity = map.ToEntity();
                this.Maps.Add(entity);
                this.EditMap(entity);
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }
        }

        private async void RefreshMaps()
        {
            await this.LoadMaps();
        }
    }
}
