namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Windows;

    using Billapong.GameConsole.Properties;

    using Configuration;
    using Game;
    using Models;
    using Models.Events;
    using Service;

    /// <summary>
    /// The loading screen view model
    /// </summary>
    public class LoadingScreenViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// Defines whether the current user is the game owner
        /// </summary>
        private readonly bool isGameOwner;

        /// <summary>
        /// The game type
        /// </summary>
        private readonly GameConfiguration.GameType gameType;

        /// <summary>
        /// The loading message
        /// </summary>
        private string loadingMessage;

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
        /// Gets or sets the current game identifier.
        /// </summary>
        /// <value>
        /// The current game identifier.
        /// </value>
        public Guid CurrentGameId { get; set; }

        /// <summary>
        /// Gets the loading message.
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
                this.OnPropertyChanged();
            }
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

            var gameStateViewModel = new GameStateViewModel(game);
            GameManager.Current.StartGame(game, gameStateViewModel);

            // Remove the current method from the GameStarted event
            GameConsoleContext.Current.GameConsoleCallback.GameStarted -= this.StartGame;

            this.SwitchWindowContent(gameStateViewModel);
        }

        /// <summary>
        /// Navigates back to the previous view
        /// </summary>
        protected override void NavigateBack()
        {
            try
            {
                if (this.CurrentGameId != Guid.Empty)
                {
                    GameConsoleContext.Current.GameConsoleServiceClient.CancelGame(this.CurrentGameId);
                }

                base.NavigateBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error);
                this.ReturnToMenu();
            }
        }
    }
}
