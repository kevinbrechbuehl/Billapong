namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Configuration;
    using WindowSelection;
    using Converter.Map;
    using Models;
    using Service;

    /// <summary>
    /// The map selection view model
    /// </summary>
    public class MapSelectionViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The proxy
        /// </summary>
        private readonly GameConsoleServiceClient proxy;

        /// <summary>
        /// The game type
        /// </summary>
        private readonly GameConfiguration.GameType gameType;

        /// <summary>
        /// The is data loading
        /// </summary>
        private bool isDataLoading = false;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel" /> class.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        public MapSelectionViewModel(GameConfiguration.GameType gameType)
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
            this.BackButtonContent = "Back to menu";

            this.gameType = gameType;
            this.proxy = new GameConsoleServiceClient();
            this.Maps = new ObservableCollection<Map>();
            this.LoadMaps();
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <value>
        /// The maps.
        /// </value>
        public ObservableCollection<Map> Maps { get; private set; }

        /// <summary>
        /// Gets the open window selection command.
        /// </summary>
        /// <value>
        /// The open window selection command.
        /// </value>
        public DelegateCommand OpenWindowSelectionCommand
        {
            get
            {
                return new DelegateCommand(this.OpenWindowSelection);
            }
        }

        /// <summary>
        /// Loads the maps.
        /// </summary>
        private async void LoadMaps()
        {
            this.IsDataLoading = true;
            var maps = await this.proxy.GetMapsAsync();
            foreach (var map in maps)
            {
                this.Maps.Add(map.ToEntity());
            }

            this.IsDataLoading = false;
        } 

        /// <summary>
        /// Opens the window selection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void OpenWindowSelection(object properties)
        {
            var map = properties as Map;
            var viewModel = WindowSelectionViewModelFactory.CreateInstance(this.gameType, map);
            this.SwitchWindowContent(viewModel);
        }
    }
}
