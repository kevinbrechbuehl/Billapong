namespace Billapong.GameConsole.ViewModels
{
    using Billapong.GameConsole.Properties;

    using Core.Client.UI;
    using Game;
    using Models;

    /// <summary>
    /// Shows the current game state
    /// </summary>
    public class GameStateViewModel : MainWindowContentViewModelBase
    {
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
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
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
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            this.ActionButtonText = Resources.CancelGame;
        }

        /// <summary>
        /// Cancels the game.
        /// </summary>
        public void CancelGame()
        {
            this.StatusMessage = Resources.GameGotCanceled;
            this.ActionButtonText = Resources.BackToMenu;
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
                    endingMessage = Resources.GameWonMessage;
                    break;
                case Player.PlayerState.Lost:
                    endingMessage = Resources.GameLostMessage;
                    break;
                default:
                    endingMessage = Resources.GameDrawMessage;
                    break;
            }

            this.StatusMessage = endingMessage;
            this.ActionButtonText = Resources.BackToMenu;
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
