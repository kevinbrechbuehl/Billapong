namespace Billapong.GameConsole.ViewModels
{
    using Core.Client.UI;
    using Game;
    using Models;

    /// <summary>
    /// Shows the current game state
    /// </summary>
    public class GameStateViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The status message
        /// </summary>
        private string statusMessage;

        /// <summary>
        /// The action button text
        /// </summary>
        private string actionButtonText;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateViewModel"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public GameStateViewModel(Game game)
        {
            this.WindowHeight = 170;
            this.WindowWidth = 300;
            this.Game = game;
        }

        /// <summary>
        /// Gets the action button command.
        /// </summary>
        /// <value>
        /// The action button command.
        /// </value>
        public DelegateCommand ActionButtonCommand
        {
            get
            {
                return new DelegateCommand(this.ActionButtonClick);
            }
        }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public Game Game { get; private set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        public string StatusMessage 
        {
            get
            {
                return this.statusMessage;
            }

            set
            {
                this.statusMessage = value;
                this.OnPropertyChanged();
            } 
        }

        /// <summary>
        /// Gets or sets the action button text.
        /// </summary>
        /// <value>
        /// The action button text.
        /// </value>
        public string ActionButtonText
        {
            get
            {
                return this.actionButtonText;
            }

            set
            {
                this.actionButtonText = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            this.ActionButtonText = "Cancel game";
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            this.StatusMessage = "Game got canceled";
            this.ActionButtonText = "Return to menu";
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame()
        {
            string endingMessage;
            switch (GameManager.Current.CurrentGame.LocalPlayer.CurrentPlayerState)
            {
                case Player.PlayerState.Won:
                    endingMessage = "Game ended. You won! :)";
                    break;
                case Player.PlayerState.Lost:
                    endingMessage = "Game ended. You lost :(";
                    break;
                default:
                    endingMessage = "Game ended. Draw!";
                    break;
            }

            this.StatusMessage = endingMessage;
            this.ActionButtonText = "Return to menu";
        }

        /// <summary>
        /// Handles the click on the action button
        /// </summary>
        private void ActionButtonClick()
        {
            if (GameManager.Current.CurrentGame.CurrentGameState == Game.GameState.Running)
            {
                GameManager.Current.CancelGame();
            }

            var gameMenuViewModel = new GameMenuViewModel();
            this.SwitchWindowContent(gameMenuViewModel);
        }
    }
}
