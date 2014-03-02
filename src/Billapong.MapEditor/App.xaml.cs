namespace Billapong.MapEditor
{
    using System.Threading;
    using System.Windows;
    using Contract.Data.Tracing;
    using Core.Client.Tracing;
    using Core.Client.UI;
    using ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // initialize tracing
            #if DEBUG
                // wait till the server is up and running
                Thread.Sleep(1000);
            #endif

            // load the main window
            (new WindowManager()).Open(new MapSelectionViewModel());

            // add events
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            Tracer.Initialize(Component.MapEditor);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            Tracer.Shutdown();
            base.OnExit(e);
        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the App control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Fehler");
            e.Handled = true;
        }
    }
}
