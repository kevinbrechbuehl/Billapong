namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Core.Client.UI;
    using Models;
    using Models.Events;
    using Window = Models.Window;

    public class GameWindowViewModel : ViewModelBase
    {
        private Window window;

        public ObservableCollection<Hole> Holes { get; private set; }

        public event EventHandler<GameFieldClickedEventArgs> GameFieldClicked = delegate { }; 

        public DelegateCommand<Point> GameFieldClickedCommand
        {
            get
            {
                return new DelegateCommand<Point>(Clicked, IsGameFieldClickable);
            }
        }

        public GameWindowViewModel(Window window)
        {
            this.window = window;
            this.Holes = new ObservableCollection<Hole>();
            foreach (var hole in this.window.Holes)
            {
                this.Holes.Add(hole);
            }
        }

        private bool IsGameFieldClickable(Point mousePosition)
        {
            return true;
        }

        private void Clicked(Point mousePosition)
        {
            // Todo (mathp2): Examining the position
            var eventArgs = new GameFieldClickedEventArgs(new Point());
            this.GameFieldClicked(this, eventArgs);   
        }
    }
}
