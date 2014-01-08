namespace Billapong.GameConsole.ViewModels
{
    using System;
    using Billapong.GameConsole.Models;

    public interface IMainWindowContentViewModel
    {
        /// <summary>
        /// Occurs when the content of the window should change
        /// </summary>
        event EventHandler<WindowContentSwitchRequestedEventArgs> WindowContentSwitchRequested;

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        int WindowHeight { get; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        int WindowWidth { get; }
    }
}
