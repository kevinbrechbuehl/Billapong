namespace Billapong.GameConsole.ViewModels
{
    using Configuration;
    using Models;

    /// <summary>
    /// The game menu view model
    /// </summary>
    public class GameMenuViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        public GameMenuViewModel()
        {
            this.WindowHeight = 350;
            this.WindowWidth = 400;
        }

        /// <summary>
        /// Gets the open map selection command.
        /// </summary>
        /// <value>
        /// The open map selection command.
        /// </value>
        public DelegateCommand OpenMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(this.OpenMapSelection);
            }
        }

        /// <summary>
        /// Gets the open game lobby command.
        /// </summary>
        /// <value>
        /// The open game lobby command.
        /// </value>
        public DelegateCommand OpenGameLobbyCommand
        {
            get
            {
                return new DelegateCommand(this.OpenGameLobby);
            }
        }

        /// <summary>
        /// Opens the map selection.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void OpenMapSelection(object parameter)
        {
            GameConfiguration.GameType gameType;

            switch (parameter != null ? parameter.ToString() : string.Empty)
            {
                case "training":
                    gameType = GameConfiguration.GameType.SinglePlayerTraining;
                    break;
                case "multiplayer":
                    gameType = GameConfiguration.GameType.MultiPlayerGame;
                    break;
                default:
                    gameType = GameConfiguration.GameType.SinglePlayerGame;
                    break;
            }

            var viewModel = new MapSelectionViewModel(gameType);
            this.SwitchWindowContent(viewModel);
        }

        /// <summary>
        /// Opens the game lobby.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void OpenGameLobby(object parameters)
        {
            var viewModel = new GameLobbyViewModel();
            this.SwitchWindowContent(viewModel);
        }
    }
}
