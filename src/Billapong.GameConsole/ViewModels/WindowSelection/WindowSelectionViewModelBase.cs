namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Billapong.GameConsole.Models;
    using Billapong.GameConsole.Views;

    /// <summary>
    /// The base implementation of the window selection view model
    /// </summary>
    public abstract class WindowSelectionViewModelBase : MainWindowContentViewModelBase, IWindowSelectionViewModel
    {
        /// <summary>
        /// The start game command
        /// </summary>
        private DelegateCommand startGameCommand;

        protected Map Map { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSelectionViewModelBase" /> class.
        /// </summary>
        /// <param name="map">The map.</param>
        protected WindowSelectionViewModelBase(Map map)
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;

            this.Map = map;

            this.Windows = new ObservableCollection<Window>();
            foreach (var window in map.Windows)
            {
                this.Windows.Add(window);
            }
        }

        /// <summary>
        /// Gets the start game command.
        /// </summary>
        /// <value>
        /// The start game command.
        /// </value>
        public virtual DelegateCommand StartGameCommand
        {
            get
            {
                return this.startGameCommand ?? (this.startGameCommand = new DelegateCommand(this.StartGame, this.CanStartGame));
            }
        }

        /// <summary>
        /// Gets the toggle window button command.
        /// </summary>
        /// <value>
        /// The toggle window button command.
        /// </value>
        public virtual DelegateCommand ToggleWindowButtonCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleWindowButton);
            }
        }

        /// <summary>
        /// Gets the back to map selection command.
        /// </summary>
        /// <value>
        /// The back to map selection command.
        /// </value>
        public virtual DelegateCommand BackToMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(this.BackToMapSelection);
            }
        }

        public virtual DelegateCommand JoinGameCommand
        {
            get
            {
                return new DelegateCommand(this.JoinGame);
            }
        }

        /// <summary>
        /// Gets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public ObservableCollection<Window> Windows { get; private set; }

        /// <summary>
        /// Changes the window back to the map selection
        /// </summary>
        /// <param name="properties">The properties.</param>
        protected virtual void BackToMapSelection(object properties)
        {
            var viewModel = new MapSelectionViewModel();
            this.OnWindowContentSwitchRequested(new WindowContentSwitchRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Toggles the window button.
        /// </summary>
        /// <param name="properties">The properties.</param>
        protected virtual void ToggleWindowButton(object properties)
        {
            this.StartGameCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="properties">The properties.</param>
        protected virtual void StartGame(object properties)
        {
            var gameWindow = new GameWindow();
            gameWindow.Show();
        }

        /// <summary>
        /// Determines whether the game can be started or not.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>The check result</returns>
        protected virtual bool CanStartGame(object properties)
        {
            return this.Windows.Any(x => x.IsVisible);
        }

        protected virtual void JoinGame(object properties)
        {
        }
    }
}
