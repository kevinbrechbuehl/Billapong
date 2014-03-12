namespace Billapong.GameConsole.ViewModels
{
    using Core.Client.UI;
    using Models.Events;

    /// <summary>
    /// The game main window view model
    /// </summary>
    public class GameMainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMainWindowViewModel"/> class.
        /// </summary>
        public GameMainWindowViewModel()
        {
            this.SwitchWindowContent(new GameMenuViewModel());
        }

        /// <summary>
        /// Gets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        public object CurrentView
        {
            get
            {
                return this.GetValue<object>();
            }

            private set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        public int WindowHeight
        {
            get
            {
                return this.GetValue<int>();
            }

            private set 
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        public int WindowWidth
        {
            get
            {
                return this.GetValue<int>();
            }
            
            private set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Swaps the wîndow content to the requested view model
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="WindowContentSwitchRequestedEventArgs"/> instance containing the event data.</param>
        public void WindowContentSwitchRequested(object sender, WindowContentSwitchRequestedEventArgs args) 
        {
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
