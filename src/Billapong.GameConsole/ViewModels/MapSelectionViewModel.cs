using Billapong.GameConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Billapong.GameConsole.ViewModels
{
    public class MapSelectionViewModel : UserControlViewModelBase
    {
        /// <summary>
        /// Gets the back to menu command.
        /// </summary>
        /// <value>
        /// The back to menu command.
        /// </value>
        public ICommand BackToMenuCommand
        {
            get
            {
                return new DelegateCommand(BackToMenu);
            }
        }

        /// <summary>
        /// Gets the open window selection command.
        /// </summary>
        /// <value>
        /// The open window selection command.
        /// </value>
        public ICommand OpenWindowSelectionCommand
        {
            get
            {
                return new DelegateCommand(OpenWindowSelection);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionViewModel"/> class.
        /// </summary>
        public MapSelectionViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
        }

        /// <summary>
        /// Changes the view back to the menu
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void BackToMenu(object properties)
        {
            var viewModel = new GameMenuViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Opens the window selection.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void OpenWindowSelection(object properties)
        {
            var viewModel = new WindowSelectionViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }
    }
}
