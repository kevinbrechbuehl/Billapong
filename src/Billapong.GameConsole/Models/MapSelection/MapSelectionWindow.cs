namespace Billapong.GameConsole.Models.MapSelection
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using Billapong.Core.Client.UI;
    using Billapong.GameConsole.Converter.Map;

    using Window = Billapong.GameConsole.Models.Window;

    public class MapSelectionWindow : NotificationObject
    {
        public bool IsClickable { get; private set; }

        public MapSelectionWindow(int x, int y, Window mapWindow, double holeDiameter)
        {
            this.X = x;
            this.Y = y;
            this.Holes = new ObservableCollection<MapSelectionWindowHole>();

            if (mapWindow != null)
            {
                this.IsClickable = true;
                this.Id = mapWindow.Id;
                foreach (var hole in mapWindow.Holes)
                {
                    this.Holes.Add(hole.ToMapSelectionWindowHole(holeDiameter));
                }
            }
            else
            {
                this.IsChecked = false;
            }

            this.SetBackground();
        }

        public long Id { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        private Brush background;

        public Brush Background
        {
            get
            {
                return this.background;
            }

            private set
            {
                this.background = value;
                this.OnPropertyChanged();
            }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
                this.SetBackground();
                
            }
        }

        public ObservableCollection<MapSelectionWindowHole> Holes { get; private set; }

        private void SetBackground()
        {
            if (this.isChecked)
            {
                this.Background = Brushes.LightBlue;
            }
            else
            {
                if (this.IsClickable)
                {
                    this.Background = Brushes.LightGray;
                }
                else
                {
                    this.Background = Brushes.White;
                }
            }
        }
    }
}
