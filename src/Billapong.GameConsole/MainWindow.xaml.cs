using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Billapong.GameConsole
{
    using System;
    using System.Collections.Generic;
    using Contract.Data.Tracing;
    using Contract.Service;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceClient client;
        
        public MainWindow()
        {
            InitializeComponent();

            this.client = new ServiceClient(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:4711"));
        }

        private void GetMaps_Clicked(object sender, RoutedEventArgs e)
        {
            var result = this.client.GetMaps();
            MessageBox.Show(string.Format("Map count is: {0}", result.Count()));
        }

        private void Log_Clicked(object sender, RoutedEventArgs e)
        {
            var proxy = ChannelFactory<ITracingService>.CreateChannel(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:4710"));

            Exception exception = null;
            try
            {
                int.Parse("lala");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var messages = new List<LogMessage>();
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 1"});
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 2" });
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Debug, Message = "Debug 3" });
            messages.Add(new LogMessage { Timestamp = DateTime.Now, Component = "Client", Sender = System.Environment.MachineName, LogLevel = LogLevel.Error, Message = exception.Message + exception.StackTrace });
            
            proxy.Log(messages);
        }
    }
}
