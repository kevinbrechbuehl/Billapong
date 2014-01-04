using Billapong.GameConsole.Models;
using Billapong.GameConsole.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Billapong.GameConsole.ViewModels
{
    public class GameMainWindowViewModel : ViewModelBase
    {
        private object currentView;
        private int windowHeight;
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
            this.SwapWindowContent(new GameMenuViewModel());
        }

        /// <summary>
        /// Swaps the wîndow content to the requested view model
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="WindowContentSwapRequestedEventArgs"/> instance containing the event data.</param>
        public void WindowContentSwapRequested(object sender, WindowContentSwapRequestedEventArgs args) {
            this.SwapWindowContent(args.ViewModel);
        }

        /// <summary>
        /// Swaps the content of the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        private void SwapWindowContent(UserControlViewModelBase viewModel)
        {
            this.CurrentView = viewModel;
            this.WindowWidth = viewModel.WindowWidth;
            this.WindowHeight = viewModel.WindowHeight;
            viewModel.WindowContentSwapRequested += WindowContentSwapRequested;
        }
    }
}
