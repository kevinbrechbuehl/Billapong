using Billapong.GameConsole.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Billapong.GameConsole.Views
{
    /// <summary>
    /// Interaction logic for GameMainWindow.xaml
    /// </summary>
    public partial class GameMainWindow : Window
    {
        public GameMainWindow()
        {
            InitializeComponent();
            this.DataContext = new GameMainWindowViewModel();
        }
    }
}
