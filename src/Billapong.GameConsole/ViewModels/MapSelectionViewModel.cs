namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using Billapong.GameConsole.Configuration;
    using Billapong.GameConsole.ViewModels.WindowSelection;
    using Converter.Map;
    using Models;
    using Service;

    public class MapSelectionViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <value>
        /// The maps.
        /// </value>
        public ObservableCollection<Map> Maps { get; private set; }

        /// <summary>
        /// Gets the back to menu command.
        /// </summary>
        /// <value>
        /// The back to menu command.
        /// </value>
        public DelegateCommand BackToMenuCommand
        {
            get
            {
                return new DelegateCommand(BackToMenu);
            }
        }

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
                return new DelegateCommand(OpenWindowSelection);
            }
        }

        private readonly GameConsoleServiceClient proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel"/> class.
        /// </summary>
        public MapSelectionViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;

            this.proxy = new GameConsoleServiceClient();
            this.Maps = new ObservableCollection<Map>();
            this.LoadMaps();
        }

        private async void LoadMaps()
        {
            var maps = await proxy.GetMapsAsync();
            foreach (var map in maps)
            {
                this.Maps.Add(map.ToEntity());
            }
        }

        /// <summary>
        /// Changes the view back to the menu
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void BackToMenu(object properties)
        {
            var viewModel = new GameMenuViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwitchRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Opens the window selection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void OpenWindowSelection(object properties)
        {
            var map = properties as Map;
            var viewModel = WindowSelectionViewModelFactory.CreateInstance(GameConfiguration.GameType.MultiPlayerGame, map);
            this.OnWindowContentSwapRequested(new WindowContentSwitchRequestedEventArgs(viewModel));
        }
    }
}
