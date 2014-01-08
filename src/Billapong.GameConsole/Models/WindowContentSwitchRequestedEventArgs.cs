using Billapong.GameConsole.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.GameConsole.Models
{
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
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public IMainWindowContentViewModel ViewModel { get; private set; }
    }
}
