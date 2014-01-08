namespace Billapong.GameConsole.ViewModels
{
    using Billapong.GameConsole.Models;

    public class GameMainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The current view
        /// </summary>
        private object currentView;

        /// <summary>
        /// The window height
        /// </summary>
        private int windowHeight;

        /// <summary>
        /// The window width
        /// </summary>
        private int windowWidth;

        /// <summary>
        /// Gets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        public object CurrentView
        {
            get { return currentView; }
            private set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        public int WindowHeight
        {
            get { return windowHeight; }
            set 
            {
                windowHeight = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMainWindowViewModel"/> class.
        /// </summary>
        public GameMainWindowViewModel()
        {
            this.SwitchWindowContent(new GameMenuViewModel());
        }

        /// <summary>
        /// Swaps the wîndow content to the requested view model
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="WindowContentSwitchRequestedEventArgs"/> instance containing the event data.</param>
        public void WindowContentSwitchRequested(object sender, WindowContentSwitchRequestedEventArgs args) {
            this.SwitchWindowContent(args.ViewModel);
        }

        /// <summary>
        /// Swaps the content of the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        private void SwitchWindowContent(IMainWindowContentViewModel viewModel)
        {
            this.CurrentView = viewModel;
            this.WindowWidth = viewModel.WindowWidth;
            this.WindowHeight = viewModel.WindowHeight;
            viewModel.WindowContentSwitchRequested += this.WindowContentSwitchRequested;
        }
    }
}
