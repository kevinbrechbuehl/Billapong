using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Billapong.GameConsole
{
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
    }
}
