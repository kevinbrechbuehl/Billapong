namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Models;
    using Models.Events;
    using Window = Models.Window;

    public class GameWindowViewModel : ViewModelBase
    {
        private Window window;

        public ObservableCollection<Hole> Holes { get; private set; }

        public event EventHandler<GameFieldClickedEventArgs> GameFieldClicked = delegate { }; 

        public DelegateCommand GameFieldClickedCommand
        {
            get
            {
                return new DelegateCommand(Clicked, IsGameFieldClickable);
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

        private bool IsGameFieldClickable(object properties)
        {
            return true;
        }

        private void Clicked(object properties)
        {
            // Todo (mathp2): Examining the position
            var eventArgs = new GameFieldClickedEventArgs(new Point());
            this.GameFieldClicked(this, eventArgs);
        }
    }
}
