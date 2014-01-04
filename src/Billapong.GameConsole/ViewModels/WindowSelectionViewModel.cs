using Billapong.GameConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Billapong.GameConsole.ViewModels
{
    public class WindowSelectionViewModel : UserControlViewModelBase
    {
        /// <summary>
        /// Gets the back to map selection command.
        /// </summary>
        /// <value>
        /// The back to map selection command.
        /// </value>
        public ICommand BackToMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(BackToMapSelection);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSelectionViewModel"/> class.
        /// </summary>
        public WindowSelectionViewModel()
        {
            this.WindowHeight = 400;
            this.WindowWidth = 500;
        }

        /// <summary>
        /// Changes the window back to the map selection
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void BackToMapSelection(object properties)
        {
            var viewModel = new MapSelectionViewModel();
            this.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }
    }
}
