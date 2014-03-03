namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.Windows;

    using Billapong.GameConsole.Properties;

    using Configuration;
    using Contract.Data.GamePlay;
    using Contract.Exceptions;
    using Core.Client.UI;
    using Service;

    /// <summary>
    /// The game lobby view model
    /// </summary>
    public class GameLobbyViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The selected lobby game
        /// </summary>
        private LobbyGame selectedLobbyGame;

        /// <summary>
        /// The join game command
        /// </summary>
        private DelegateCommand joinGameCommand;

        /// <summary>
        /// The is data loading
        /// </summary>
        private bool isDataLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLobbyViewModel"/> class.
        /// </summary>
        public GameLobbyViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
            this.BackButtonContent = Resources.BackToMenu;
            this.OpenGames = new ObservableCollection<LobbyGame>();
            this.LoadOpenGames();
        }

        /// <summary>
        /// Gets or sets the selected lobby game.
        /// </summary>
        /// <value>
        /// The selected lobby game.
        /// </value>
        public LobbyGame SelectedLobbyGame
        {
            get
            {
                return this.selectedLobbyGame;
            }

            set
            {
                this.selectedLobbyGame = value;
                this.OnPropertyChanged();
                this.JoinGameCommand.RaiseCanExecuteChanged();
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
        /// Gets the join game command.
        /// </summary>
        /// <value>
        /// The join game command.
        /// </value>
        public DelegateCommand JoinGameCommand
        {
            get
            {
                if (this.joinGameCommand == null)
                {
                    this.JoinGameCommand = new DelegateCommand(this.JoinGame, () => this.SelectedLobbyGame != null);
                }

                return this.joinGameCommand;
            }

            private set
            {
                this.joinGameCommand = value;
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
        private void JoinGame()
        {
            try
            {
                var loadingScreen = new LoadingScreenViewModel(Resources.JoiningGameMessage, GameConfiguration.GameType.MultiPlayerGame)
                                        {
                                            CurrentGameId = this .SelectedLobbyGame.Id
                                        };

                GameConsoleContext.Current.GameConsoleCallback.GameStarted += loadingScreen.StartGame;
                GameConsoleContext.Current.GameConsoleServiceClient.JoinGame(this.SelectedLobbyGame.Id, Settings.Default.PlayerName);
                this.SwitchWindowContent(loadingScreen);
            }
            catch (FaultException<GameNotOpenException>)
            {
                MessageBox.Show(Resources.GameAlreadyOpened, Resources.Error);
            }
        }

        /// <summary>
        /// Loads the open games.
        /// </summary>
        private async void LoadOpenGames()
        {
            this.IsDataLoading = true;
            this.OpenGames.Clear();
            var games = await GameConsoleContext.Current.GameConsoleServiceClient.GetLobbyGamesAsync();
            foreach (var game in games)
            {
                this.OpenGames.Add(game);
            }

            this.IsDataLoading = false;
        }
    }
}
