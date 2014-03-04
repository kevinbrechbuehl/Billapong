namespace Billapong.Core.Client.Helper
{
    using System;
    using System.Windows;

    /// <summary>
    /// Thread helper to forward actions to the UI thread
    /// </summary>
    public static class ThreadContext
    {
        /// <summary>
        /// Invokes the actio on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void InvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// Begins the invoke on the method in the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void BeginInvokeOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }
    }
}
