namespace Billapong.Core.Client.Helper
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Billapong.Core.Client.Exceptions;
    using Billapong.Core.Client.Properties;
    using Billapong.Core.Client.Tracing;

    /// <summary>
    /// Global application helper methods.
    /// </summary>
    public static class ApplicationHelpers
    {
        /// <summary>
        /// Handles the server exception asynchronous.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="withoutShutdown">if set to <c>true</c> it should only handle error without application shutdown.</param>
        public static async void HandleServerException(ServerUnavailableException ex, bool withoutShutdown = false)
        {
            await LogError("Server not available", ex);
            if (!withoutShutdown)
            {
                ShutdownApplication(Resources.ServerUnavailable);
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ShutdownApplication(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = Resources.UnexpectedError;
            }

            MessageBox.Show(message, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Logs the error to the current tracer.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        /// <returns>Async task</returns>
        private static async Task LogError(string message, Exception ex)
        {
            await Tracer.Error(message, ex);
        }
    }
}
