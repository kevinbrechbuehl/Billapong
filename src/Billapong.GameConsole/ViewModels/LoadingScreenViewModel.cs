namespace Billapong.GameConsole.ViewModels
{
    using System.Windows;
    using Service;

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
        /// Initializes a new instance of the <see cref="LoadingScreenViewModel"/> class.
        /// </summary>
        /// <param name="loadingMessage">The loading message.</param>
        /// <param name="isGameOwner">if set to <c>true</c> the current user is the game owner.</param>
        public LoadingScreenViewModel(string loadingMessage, bool isGameOwner = false)
        {
            this.WindowWidth = 200;
            this.WindowHeight = 200;
            this.LoadingMessage = loadingMessage;
            this.BackButtonContent = "Cancel";

            this.isGameOwner = isGameOwner;
        }

        protected override void NavigateBack(object properties)
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

            base.NavigateBack(properties);
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
