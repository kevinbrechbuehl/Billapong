namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
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
            this.proxy.JoinGame(this.OpenGames.First().Id, "Second player :)");
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
            this.OpenGames.Clear();
            var games = await this.proxy.GetLobbyGamesAsync();
            foreach (var game in games)
            {
                this.OpenGames.Add(game);
            }
        }
    }
}
