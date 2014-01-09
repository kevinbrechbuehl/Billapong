namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Billapong.GameConsole.Models;
    using Core.Client.Tracing;
    using Service;

    /// <summary>
    /// The game menu view model
    /// </summary>
    public class GameMenuViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        public GameMenuViewModel()
        {
            this.WindowHeight = 350;
            this.WindowWidth = 400;
        }

        /// <summary>
        /// Gets the open map selection command.
        /// </summary>
        /// <value>
        /// The open map selection command.
        /// </value>
        public DelegateCommand OpenMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(this.OpenMapSelection);
            }
        }

        /// <summary>
        /// Opens the map selection.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void OpenMapSelection(object parameter)
        {
            var viewModel = new MapSelectionViewModel();
            this.OnWindowContentSwitchRequested(new WindowContentSwitchRequestedEventArgs(viewModel));
        }
    }
}
