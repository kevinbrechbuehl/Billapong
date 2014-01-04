using Billapong.GameConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.GameConsole.ViewModels
{
    public abstract class UserControlViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Occurs when the content of the window should change
        /// </summary>
        public event EventHandler<WindowContentSwapRequestedEventArgs> WindowContentSwapRequested = delegate {};

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
        /// Raises the <see cref="E:WindowContentSwapRequested" /> event.
        /// </summary>
        /// <param name="e">The <see cref="WindowContentSwapRequestedEventArgs"/> instance containing the event data.</param>
        protected void OnWindowContentSwapRequested(WindowContentSwapRequestedEventArgs e)
        {
            if (WindowContentSwapRequested != null)
            {
                WindowContentSwapRequested(this, e);
            }
        }
    }
}
