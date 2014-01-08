namespace Billapong.GameConsole.Views
{
    using System.Windows;
    using Billapong.GameConsole.ViewModels;

    /// <summary>
    /// Interaction logic for GameMainWindow.xaml
    /// </summary>
    public partial class GameMainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMainWindow"/> class.
        /// </summary>
        public GameMainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new GameMainWindowViewModel();
        }
    }
}
