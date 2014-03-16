namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Billapong.Core.Client.Exceptions;
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
        /// The current game identifier
        /// </summary>
        private Guid currentGameId;

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
            this.BackButtonContent = Resources.Cancel;
            this.gameType = gameType;
            this.isGameOwner = isGameOwner;

            Application.Current.MainWindow.Closing += this.OnWindowClosing;
        }

        /// <summary>
        /// Gets or sets the current game identifier.
        /// </summary>
        /// <value>
        /// The current game identifier.
        /// </value>
        public Guid CurrentGameId 
        {
            get
            {
                return this.currentGameId;
            }

            set
            {
                this.currentGameId = value;
                GameConsoleContext.Current.StartKeepGameAlive(value);
            } 
        }

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
                return this.GetValue<string>();
            }

            private set
            {
                this.SetValue(value);
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
            Application.Current.MainWindow.Closing -= this.OnWindowClosing;
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
                    GameConsoleContext.Current.GameConsoleServiceClient.CancelGame(this.CurrentGameId, this.isGameOwner, false);
                }

                base.NavigateBack();
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error);
                this.ReturnToMenu();
            }
        }

        /// <summary>
        /// Called when the user closes the main window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        protected async void OnWindowClosing(object sender, CancelEventArgs args)
        {
            if (!this.CurrentGameId.Equals(Guid.Empty)) 
            {
                try
                {
                    await
                        GameConsoleContext.Current.GameConsoleServiceClient.CancelGameAsync(
                            this.CurrentGameId,
                            this.isGameOwner,
                            false);
                }
                catch (ServerUnavailableException ex)
                {
                    this.HandleServerException(ex);
                }
            }
        }
    }
}
