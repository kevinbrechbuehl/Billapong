namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Billapong.Contract.Data.GamePlay;
    using Billapong.GameConsole.Service;

    /// <summary>
    /// The game lobby view model
    /// </summary>
    public class GameLobbyViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The proxy
        /// </summary>
        private readonly GameConsoleServiceClient proxy;

        /// <summary>
        /// The selected lobby game
        /// </summary>
        private LobbyGame selectedLobbyGame;

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
        /// Initializes a new instance of the <see cref="GameLobbyViewModel"/> class.
        /// </summary>
        public GameLobbyViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
            this.BackButtonContent = "Back to menu";
            this.OpenGames = new ObservableCollection<LobbyGame>();

            this.proxy = new GameConsoleServiceClient();
            this.LoadOpenGames();
        }

        /// <summary>
        /// Gets the join game command.
        /// </summary>
        /// <value>
        /// The join game command.
        /// </value>
        public DelegateCommand JoinGameCommand
        {
            get
            {
                return new DelegateCommand(this.JoinGame);
            }
        }

        /// <summary>
        /// Gets the game selected command.
        /// </summary>
        /// <value>
        /// The game selected command.
        /// </value>
        public DelegateCommand GameSelectedCommand
        {
            get
            {
                // Todo (mathp2): Korrekt auslösen
                return new DelegateCommand(this.SetSelectedLobbyGame);
            }
        }

        /// <summary>
        /// Gets the refresh games command.
        /// </summary>
        /// <value>
        /// The refresh games command.
        /// </value>
        public DelegateCommand RefreshGamesCommand
        {
            get
            {
                return new DelegateCommand(this.LoadOpenGames);
            }
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <value>
        /// The maps.
        /// </value>
        public ObservableCollection<LobbyGame> OpenGames { get; private set; }

        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void JoinGame(object properties)
        {
            // Todo (mathp2): Join the selected game
            this.proxy.JoinGame(this.OpenGames.First().Id, Properties.Settings.Default.PlayerName);
        }

        /// <summary>
        /// Sets the selected lobby game.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void SetSelectedLobbyGame(object properties)
        {
            this.selectedLobbyGame = properties as LobbyGame;
        }

        /// <summary>
        /// Loads the open games.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private async void LoadOpenGames(object properties = null)
        {
            this.IsDataLoading = true;
            this.OpenGames.Clear();
            var games = await this.proxy.GetLobbyGamesAsync();
            foreach (var game in games)
            {
                this.OpenGames.Add(game);
            }
            this.IsDataLoading = false;
        }
    }
}
