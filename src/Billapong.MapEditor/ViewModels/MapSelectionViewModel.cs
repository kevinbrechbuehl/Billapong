namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Billapong.Core.Client.Exceptions;
    using Billapong.Core.Client.Tracing;
    using Billapong.MapEditor.Properties;
    using Converter;
    using Core.Client.UI;
    using Models;
    using Services;

    /// <summary>
    /// Map selection view model.
    /// </summary>
    public class MapSelectionViewModel : ViewModelBase
    {
        /// <summary>
        /// The session identifier
        /// </summary>
        private readonly Guid sessionId;

        /// <summary>
        /// The map editor service proxy
        /// </summary>
        private MapEditorServiceClient proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel"/> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public MapSelectionViewModel(Guid sessionId)
        {
            this.sessionId = sessionId;
            this.Initialize();
        }

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
                return this.GetValue<bool>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <value>
        /// The maps.
        /// </value>
        public ObservableCollection<Map> Maps { get; private set; }

        /// <summary>
        /// Gets the create new map command.
        /// </summary>
        /// <value>
        /// The create new map command.
        /// </value>
        public DelegateCommand CreateNewMapCommand
        {
            get
            {
                return new DelegateCommand(this.CreateNewMap);
            }
        }

        /// <summary>
        /// Gets the refresh maps command.
        /// </summary>
        /// <value>
        /// The refresh maps command.
        /// </value>
        public DelegateCommand RefreshMapsCommand
        {
            get
            {
                return new DelegateCommand(this.RefreshMaps);
            }
        }

        /// <summary>
        /// Gets the delete map command.
        /// </summary>
        /// <value>
        /// The delete map command.
        /// </value>
        public DelegateCommand<Map> DeleteMapCommand
        {
            get
            {
                return new DelegateCommand<Map>(this.DeleteMap);
            }
        }

        /// <summary>
        /// Gets the edit map command.
        /// </summary>
        /// <value>
        /// The edit map command.
        /// </value>
        public DelegateCommand<Map> EditMapCommand
        {
            get
            {
                return new DelegateCommand<Map>(this.EditMap);
            }
        }

        /// <summary>
        /// Gets called when the connected view closes
        /// </summary>
        public override void CloseCallback()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Initializes this map.
        /// </summary>
        private async void Initialize()
        {
            this.proxy = new MapEditorServiceClient(this.sessionId);
            this.Maps = new ObservableCollection<Map>();
            await this.LoadMaps();
        }

        /// <summary>
        /// Loads the maps from the database.
        /// </summary>
        /// <returns>Async task</returns>
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

        /// <summary>
        /// Deletes the map by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Async task</returns>
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

        /// <summary>
        /// Deletes the map.
        /// </summary>
        /// <param name="map">The map.</param>
        private async void DeleteMap(Map map)
        {
            if (MessageBox.Show(string.Format(Resources.DeleteMapQuestion, ((Map)map).Name), Resources.ConfirmationTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await this.DeleteMap(map.Id);
            }
        }

        /// <summary>
        /// Edits the map and open map editing window.
        /// </summary>
        /// <param name="map">The map.</param>
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

            this.WindowManager.Open(new MapEditViewModel(mapToEdit, this.sessionId));
        }

        /// <summary>
        /// Creates a new map.
        /// </summary>
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

        /// <summary>
        /// Refreshes the maps.
        /// </summary>
        private async void RefreshMaps()
        {
            await this.LoadMaps();
        }
    }
}
