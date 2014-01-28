namespace Billapong.MapEditor.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;
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

        private readonly MapEditorServiceClient proxy;

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

        public DelegateCommand DeleteMapCommand
        {
            get
            {
                return new DelegateCommand(this.DeleteMap);
            }
        }

        public DelegateCommand EditMapCommand
        {
            get
            {
                return new DelegateCommand(this.EditMap);
            }
        }

        public MapSelectionViewModel()
        {
            this.proxy = new MapEditorServiceClient();
            this.Maps = new ObservableCollection<Map>();
            this.LoadMaps();
        }

        private async void LoadMaps()
        {
            this.IsDataLoading = true;
            var maps = await this.proxy.GetMapsAsync();
            this.Maps.Clear();
            foreach (var map in maps)
            {
                this.Maps.Add(map.ToEntity());
            }

            this.IsDataLoading = false;
        }

        private void DeleteMap(object map)
        {
            // todo (breck1): decouple this from the viewmodel, i.e. with http://deanchalk.com/2010/05/06/wpf-mvvm-simple-messagebox-show-with-action-func/
            if (MessageBox.Show(string.Format("Are you sure you want to delete the map '{0}'?", ((Map)map).Name),
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBox.Show("delete");    
            }
        }

        private void EditMap(object map)
        {
            MessageBox.Show(string.Format("edit map {0}", ((Map)map).Name));
        }

        private void CreateNewMap(object parameters)
        {
            MessageBox.Show("create new map");
        }

        private void RefreshMaps(object parameters)
        {
            this.LoadMaps();
        }
    }
}
