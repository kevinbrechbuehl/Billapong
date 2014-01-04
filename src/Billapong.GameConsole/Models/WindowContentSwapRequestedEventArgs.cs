using Billapong.GameConsole.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.GameConsole.Models
{
    public class WindowContentSwapRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowContentSwapRequestedEventArgs"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public WindowContentSwapRequestedEventArgs(UserControlViewModelBase viewModel)
        {
            this.ViewModel = viewModel;
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public UserControlViewModelBase ViewModel { get; private set; }
    }
}
