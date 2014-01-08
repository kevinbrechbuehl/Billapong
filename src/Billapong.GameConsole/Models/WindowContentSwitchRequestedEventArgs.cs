namespace Billapong.GameConsole.Models
{
    using System;

    using Billapong.GameConsole.ViewModels;

    /// <summary>
    /// The arguments of a WindowContentSwitchRequested event
    /// </summary>
    public class WindowContentSwitchRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowContentSwitchRequestedEventArgs"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public WindowContentSwitchRequestedEventArgs(IMainWindowContentViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public IMainWindowContentViewModel ViewModel { get; private set; }
    }
}
