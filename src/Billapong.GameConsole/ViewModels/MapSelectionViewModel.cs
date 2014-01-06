using Billapong.GameConsole.Models;
using System.Windows.Input;

namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Service;

    public class MapSelectionViewModel : UserControlViewModelBase
    {
        public ObservableCollection<string> Maps { get; set; }

        /// <summary>
        /// Gets the back to menu command.
        /// </summary>
        /// <value>
        /// The back to menu command.
        /// </value>
        public ICommand BackToMenuCommand
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
        public ICommand OpenWindowSelectionCommand
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

            Maps = new ObservableCollection<string>();
            /*Maps.Add("Map 1");
            Maps.Add("Map 2");
            Maps.Add("Map 3");
            Maps.Add("Map 4");
            Maps.Add("Map 5");*/

            var client = new GameConsoleServiceClient();
            var maps = client.GetMaps().ToList();

            foreach (var map in maps)
            {
                Maps.Add(map.Name);
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
            var viewModel = new WindowSelectionViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }
    }
}
