namespace Billapong.GameConsole.Views
{
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        public GameMenu()
        {
            InitializeComponent();
            this.DataContext = new GameMenuViewModel();
        }
    }
}
