using System.Windows;

namespace Billapong.GameConsole
{
    using Contract.Data.Tracing;
    using Core.Client.Tracing;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            Tracer.Initialize(Component.GameConsole);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Fehler");
            e.Handled = true;
        }
    }
}
