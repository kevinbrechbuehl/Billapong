namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using Models;

    public class GameWindowViewModel : ViewModelBase
    {
        private Window window;

        public ObservableCollection<Hole> Holes { get; private set; }

        public GameWindowViewModel(Window window)
        {
            this.window = window;
            this.Holes = new ObservableCollection<Hole>();
            foreach (var hole in this.window.Holes)
            {
                this.Holes.Add(hole);
            }
        }
    }
}
