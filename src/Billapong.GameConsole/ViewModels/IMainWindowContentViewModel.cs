namespace Billapong.GameConsole.ViewModels
{
    using System;
    using Billapong.GameConsole.Models;

    /// <summary>
    /// Interface for all view models that represents some controls within the main window
    /// </summary>
    public interface IMainWindowContentViewModel
    {
        /// <summary>
        /// Occurs when the content of the window should change
        /// </summary>
        event EventHandler<WindowContentSwitchRequestedEventArgs> WindowContentSwitchRequested;

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        int WindowHeight { get; }

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        int WindowWidth { get; }
    }
}
