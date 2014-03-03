namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Windows;
    using Core.Client.UI;
    using Models.Events;

    /// <summary>
    /// Provides some basic properties and methods for the main window content view models
    /// </summary>
    public abstract class MainWindowContentViewModelBase : ViewModelBase, IMainWindowContentViewModel
    {
        /// <summary>
        /// Occurs when the content of the window should change
        /// </summary>
        public event EventHandler<WindowContentSwitchRequestedEventArgs> WindowContentSwitchRequested = delegate { };

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        public int WindowHeight { get; protected set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        public int WindowWidth { get;  protected set; }

        /// <summary>
        /// Gets or sets the previous view model.
        /// </summary>
        /// <value>
        /// The previous view model.
        /// </value>
        public IMainWindowContentViewModel PreviousViewModel { get; set; }

        /// <summary>
        /// Gets the back button visibility.
        /// </summary>
        /// <value>
        /// The back button visibility.
        /// </value>
        public Visibility BackButtonVisibility
        {
            get
            {
                return this.PreviousViewModel != null ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Gets or sets the content of the back button.
        /// </summary>
        /// <value>
        /// The content of the back button.
        /// </value>
        public string BackButtonContent { get; set; }

        /// <summary>
        /// Gets the navigate back command.
        /// </summary>
        /// <value>
        /// The navigate back command.
        /// </value>
        public virtual DelegateCommand NavigateBackCommand
        {
            get
            {
                return new DelegateCommand(this.NavigateBack);
            }
        }

        /// <summary>
        /// Switches the content of the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        protected void SwitchWindowContent(IMainWindowContentViewModel viewModel)
        {
            if (viewModel.PreviousViewModel == null)
            {
                viewModel.PreviousViewModel = this;
            }

            this.OnWindowContentSwitchRequested(new WindowContentSwitchRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Navigates back to the previous view
        /// </summary>
        protected virtual void NavigateBack()
        {
            if (this.PreviousViewModel != null)
            {
                this.SwitchWindowContent(this.PreviousViewModel);
            }
        }

        /// <summary>
        /// Returns to menu.
        /// </summary>
        protected virtual void ReturnToMenu()
        {
            var gameMenuViewModel = new GameMenuViewModel();
            this.SwitchWindowContent(gameMenuViewModel);
        }

        /// <summary>
        /// Raises the <see cref="E:WindowContentSwitchRequested" /> event.
        /// </summary>
        /// <param name="e">The <see cref="WindowContentSwitchRequestedEventArgs"/> instance containing the event data.</param>
        private void OnWindowContentSwitchRequested(WindowContentSwitchRequestedEventArgs e)
        {
            if (this.WindowContentSwitchRequested != null)
            {
                this.WindowContentSwitchRequested(this, e);
            }
        }
    }
}
