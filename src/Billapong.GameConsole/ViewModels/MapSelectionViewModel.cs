namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Billapong.GameConsole.Game;
    using Configuration;
    using Converter.Map;
    using Core.Client.UI;
    using Models;
    using Service;

    /// <summary>
    /// The map selection view model
    /// </summary>
    public class MapSelectionViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The game type
        /// </summary>
        private readonly GameConfiguration.GameType gameType;

        /// <summary>
        /// The is data loading
        /// </summary>
        private bool isDataLoading;

        /// <summary>
        /// The window selection view model
        /// </summary>
        private WindowSelectionViewModel windowSelectionViewModel;

        /// <summary>
        /// The open game command
        /// </summary>
        private DelegateCommand openGameCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel" /> class.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        public MapSelectionViewModel(GameConfiguration.GameType gameType)
        {
            this.WindowHeight = 380;
            this.WindowWidth = 700;
            this.BackButtonContent = "Back to menu";

            this.gameType = gameType;
            this.Maps = new ObservableCollection<Map>();
            this.LoadMaps();
        }

        /// <summary>
        /// Gets the window selection view model.
        /// </summary>
        /// <value>
        /// The window selection view model.
        /// </value>
        public WindowSelectionViewModel WindowSelectionViewModel
        {
            get
            {
                return this.windowSelectionViewModel;
            }

            private set
            {
                this.windowSelectionViewModel = value;
                this.OnPropertyChanged();
            }
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
                return this.isDataLoading;
            }

            set
            {
                this.isDataLoading = value;
                this.OnPropertyChanged();
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
        /// Gets or sets the selected map.
        /// </summary>
        /// <value>
        /// The selected map.
        /// </value>
        public Map SelectedMap { get; set; }

        /// <summary>
        /// Gets the map selection changed command.
        /// </summary>
        /// <value>
        /// The map selection changed command.
        /// </value>
        public DelegateCommand MapSelectionChangedCommand
        {
            get
            {
                return new DelegateCommand(this.MapSelectionChanged);
            }
        }

        /// <summary>
        /// Gets the open game command.
        /// </summary>
        /// <value>
        /// The open game command.
        /// </value>
        public DelegateCommand OpenGameCommand
        {
            get
            {
                return this.openGameCommand ?? (this.openGameCommand = new DelegateCommand(this.OpenGame, this.CanOpenGame));
            }
        }

        /// <summary>
        /// Gets called when the selected map changes
        /// </summary>
        private void MapSelectionChanged()
        {
            this.WindowSelectionViewModel = new WindowSelectionViewModel(this.SelectedMap);
            this.WindowSelectionViewModel.WindowSelectionChanged += this.WindowSelectionChanged;
        }

        /// <summary>
        /// Loads the maps.
        /// </summary>
        private async void LoadMaps()
        {
            this.IsDataLoading = true;
            var maps = await GameConsoleContext.Current.GameConsoleServiceClient.GetMapsAsync();
            foreach (var map in maps)
            {
                this.Maps.Add(map.ToEntity());
            }

            this.IsDataLoading = false;
        }

        /// <summary>
        /// Gets called when the selected windows changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WindowSelectionChanged(object sender, EventArgs args)
        {
            this.OpenGameCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Determines whether the game can be opened in the current state.
        /// </summary>
        /// <returns>The determination result</returns>
        private bool CanOpenGame()
        {
            return this.windowSelectionViewModel != null && this.windowSelectionViewModel.SelectedWindows.Any();
        }

        /// <summary>
        /// Opens the game.
        /// </summary>
        private async void OpenGame()
        {
            IMainWindowContentViewModel nextWindow = null;

            foreach (var window in this.WindowSelectionViewModel.SelectedWindows)
            {
                var mapWindow = this.SelectedMap.Windows.FirstOrDefault(w => w.Id == window.Id);
                if (mapWindow != null)
                {
                    mapWindow.IsOwnWindow = window.IsChecked;
                }
            }

            // todo (mathp2): Implement behaviour for the single player mode
            switch (this.gameType)
            {
                case GameConfiguration.GameType.SinglePlayerTraining:
                    var game = new Game();
                    game.Init(new Guid(), this.SelectedMap, null, true, true, GameConfiguration.GameType.SinglePlayerTraining);
                    var gameStateViewModel = new GameStateViewModel(game);
                    GameManager.Current.StartGame(game, gameStateViewModel);
                    nextWindow = gameStateViewModel;
                    break;
                case GameConfiguration.GameType.MultiPlayerGame:
                    var loadingScreenViewModel = new LoadingScreenViewModel("Waiting for opponent...", GameConfiguration.GameType.MultiPlayerGame, true);
                    GameConsoleContext.Current.GameConsoleCallback.GameStarted += loadingScreenViewModel.StartGame;
                    loadingScreenViewModel.CurrentGameId = await GameConsoleContext.Current.GameConsoleServiceClient.OpenGameAsync(this.SelectedMap.Id, this.WindowSelectionViewModel.SelectedWindows.Select(x => x.Id), Properties.Settings.Default.PlayerName);
                    nextWindow = loadingScreenViewModel;
                    break;
            }

            this.SwitchWindowContent(nextWindow);
        }
    }
}
