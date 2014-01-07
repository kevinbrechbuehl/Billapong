namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Models;
    using Views;

    public class WindowSelectionViewModel : UserControlViewModelBase
    {
        /// <summary>
        /// The start game command
        /// </summary>
        private DelegateCommand startGameCommand;

        /// <summary>
        /// Gets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public ObservableCollection<Window> Windows { get; private set; }

        /// <summary>
        /// Gets the back to map selection command.
        /// </summary>
        /// <value>
        /// The back to map selection command.
        /// </value>
        public DelegateCommand BackToMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(BackToMapSelection);
            }
        }

        /// <summary>
        /// Gets the start game command.
        /// </summary>
        /// <value>
        /// The start game command.
        /// </value>
        public DelegateCommand StartGameCommand
        {
            get
            {
                return startGameCommand ?? (startGameCommand = new DelegateCommand(StartGame, CanStartGame));
            }
        }

        /// <summary>
        /// Gets the toggle window button command.
        /// </summary>
        /// <value>
        /// The toggle window button command.
        /// </value>
        public DelegateCommand ToggleWindowButtonCommand
        {
            get
            {
                return new DelegateCommand(ToggleWindowButton);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSelectionViewModel"/> class.
        /// </summary>
        public WindowSelectionViewModel(Map map)
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;

            this.Windows = new ObservableCollection<Window>();
            foreach (var window in map.Windows)
            {
                this.Windows.Add(window);
            }
        }

        /// <summary>
        /// Toggles the window button.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void ToggleWindowButton(object properties)
        {
            this.StartGameCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Changes the window back to the map selection
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void BackToMapSelection(object properties)
        {
            var viewModel = new MapSelectionViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void StartGame(object properties)
        {
            var gameWindow = new GameWindow();
            gameWindow.Show();
        }

        /// <summary>
        /// Determines whether the game can be started or not.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>The check result</returns>
        private bool CanStartGame(object properties)
        {
            return this.Windows.Any(x => x.IsVisible);
        }
    }
}
