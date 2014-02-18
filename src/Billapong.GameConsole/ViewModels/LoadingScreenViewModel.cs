namespace Billapong.GameConsole.ViewModels
{
    using System.Windows;
    using Configuration;
    using Game;
    using Models;
    using Models.Events;

    /// <summary>
    /// The loading screen view model
    /// </summary>
    public class LoadingScreenViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The loading message
        /// </summary>
        private string loadingMessage;

        /// <summary>
        /// Defines whether the current user is the game owner
        /// </summary>
        private readonly bool isGameOwner;

        /// <summary>
        /// The game type
        /// </summary>
        private GameConfiguration.GameType gameType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingScreenViewModel" /> class.
        /// </summary>
        /// <param name="loadingMessage">The loading message.</param>
        /// <param name="gameType">Type of the game.</param>
        /// <param name="isGameOwner">if set to <c>true</c> the current user is the game owner.</param>
        public LoadingScreenViewModel(string loadingMessage, GameConfiguration.GameType gameType, bool isGameOwner = false)
        {
            this.WindowWidth = 200;
            this.WindowHeight = 200;
            this.LoadingMessage = loadingMessage;
            this.BackButtonContent = "Cancel";
            this.gameType = gameType;
            this.isGameOwner = isGameOwner;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameStartedEventArgs"/> instance containing the event data.</param>
        public void StartGame(object sender, GameStartedEventArgs args)
        {
            var game = new Game();
            game.Init(args.GameId, args.Map, args.Opponent, args.StartGame, this.isGameOwner, this.gameType);


            var gameStateViewModel = new GameStateViewModel();
            GameManager.Current.StartGame(game);

            this.SwitchWindowContent(gameStateViewModel);

        }

        protected override void NavigateBack()
        {

            // Todo (mathp2): Offenes Spiel schliessen oder verlassen
            if (this.isGameOwner)
            {
                MessageBox.Show("Todo: Now we have to cancel the game on the server");
            }
            else
            {
                MessageBox.Show("Todo: Stop joining game?");
            }

            base.NavigateBack();
        }

        /// <summary>
        /// Gets or sets the loading message.
        /// </summary>
        /// <value>
        /// The loading message.
        /// </value>
        public string LoadingMessage
        {
            get
            {
                return this.loadingMessage;
            }

            private set
            {
                this.loadingMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
