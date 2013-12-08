using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Billapong.Contract.Service;

namespace Billapong.GameConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetMaps_Clicked(object sender, RoutedEventArgs e)
        {
            var proxy = ChannelFactory<IConsoleService>.CreateChannel(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:4711"));

            var result = proxy.GetMaps();
            tbMapCount.Text = result.Count().ToString();
        }
    }
}
