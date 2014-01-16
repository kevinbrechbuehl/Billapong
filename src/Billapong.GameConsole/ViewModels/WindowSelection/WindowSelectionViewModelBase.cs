namespace Billapong.GameConsole.ViewModels.WindowSelection
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Models;
    using Views;

    /// <summary>
    /// The base implementation of the window selection view model
    /// </summary>
    public abstract class WindowSelectionViewModelBase : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The start game command
        /// </summary>
        private DelegateCommand startGameCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSelectionViewModelBase" /> class.
        /// </summary>
        /// <param name="map">The map.</param>
        protected WindowSelectionViewModelBase(Map map)
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
            this.BackButtonContent = "Back to map selection";
            this.Map = map;
            this.WindowRows = new ObservableCollection<KeyValuePair<int, IEnumerable<Window>>>();
            foreach (var row in map.Rows)
            {
                this.WindowRows.Add(row);
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
        /// Gets the windows.
        /// </summary>
        /// <value>
        /// The windows.
        /// </value>
        public ObservableCollection<KeyValuePair<int, IEnumerable<Window>>> WindowRows { get; private set; }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        protected Map Map { get; private set; }

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
            return this.WindowRows.Any(x => x.Value.Any(w => w.IsVisible));
        }
    }
}
