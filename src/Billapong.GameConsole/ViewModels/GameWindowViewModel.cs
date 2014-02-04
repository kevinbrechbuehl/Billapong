namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using Core.Client.UI;
    using Models;
    using Models.Events;
    using Window = Models.Window;

    public class GameWindowViewModel : ViewModelBase
    {
        private Window window;

        public ObservableCollection<Hole> Holes { get; private set; }

        public ObservableCollection<Ball> Balls { get; private set; }

        public CompositeCollection CanvasElements { get; private set; }

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
            this.Balls = new ObservableCollection<Ball>();
            this.CanvasElements = new CompositeCollection();
            foreach (var hole in this.window.Holes)
            {
                this.Holes.Add(hole);
            }

            var ball = new Ball();
            ball.Left = 150;
            ball.Top = 150;
            this.Balls.Add(ball);

            this.CanvasElements.Add(new CollectionContainer() { Collection = this.Holes });
            this.CanvasElements.Add(new CollectionContainer() { Collection = this.Balls });
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
