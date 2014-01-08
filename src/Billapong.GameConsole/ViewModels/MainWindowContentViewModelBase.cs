using Billapong.GameConsole.Models;
using System;

namespace Billapong.GameConsole.ViewModels
{
    public abstract class MainWindowContentViewModelBase : ViewModelBase, IMainWindowContentViewModel
    {
        /// <summary>
        /// Occurs when the content of the window should change
        /// </summary>
        public event EventHandler<WindowContentSwitchRequestedEventArgs> WindowContentSwitchRequested = delegate {};

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
        /// Raises the <see cref="E:WindowContentSwitchRequested" /> event.
        /// </summary>
        /// <param name="e">The <see cref="WindowContentSwitchRequestedEventArgs"/> instance containing the event data.</param>
        protected void OnWindowContentSwapRequested(WindowContentSwitchRequestedEventArgs e)
        {
            if (this.WindowContentSwitchRequested != null)
            {
                this.WindowContentSwitchRequested(this, e);
            }
        }
    }
}
