namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Converter.Map;
    using Models;
    using Service;

    public class MapSelectionViewModel : UserControlViewModelBase
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel"/> class.
        /// </summary>
        public MapSelectionViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;

            this.Maps = new ObservableCollection<Map>();
            

            var client = new GameConsoleServiceClient();
            var maps = client.GetMaps().ToList();

            foreach (var map in maps)
            {
                Maps.Add(MapConverter.ToEntity(map));
            }
        }

        /// <summary>
        /// Changes the view back to the menu
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void BackToMenu(object properties)
        {
            var viewModel = new GameMenuViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Opens the window selection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void OpenWindowSelection(object properties)
        {
            var viewModel = new WindowSelectionViewModel((Map)properties);
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }
    }
}
