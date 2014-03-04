namespace Billapong.GameConsole.Models.MapSelection
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using Billapong.Core.Client.UI;
    using Billapong.GameConsole.Converter.Map;
    using Window = Billapong.GameConsole.Models.Window;

    /// <summary>
    /// The map selection window
    /// </summary>
    public class MapSelectionWindow : NotificationObject
    {
        /// <summary>
        /// The background
        /// </summary>
        private Brush background;

        /// <summary>
        /// Indicates whether the window is checked
        /// </summary>
        private bool isChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSelectionWindow"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="mapWindow">The map window.</param>
        /// <param name="holeDiameter">The hole diameter.</param>
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

        /// <summary>
        /// Gets a value indicating whether the window is clickable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the window is clickable; otherwise, <c>false</c>.
        /// </value>
        public bool IsClickable { get; private set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether the window is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the window is checked; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public ObservableCollection<MapSelectionWindowHole> Holes { get; private set; }

        /// <summary>
        /// Sets the background.
        /// </summary>
        private void SetBackground()
        {
            if (this.isChecked)
            {
                this.Background = Brushes.LightBlue;
            }
            else
            {
                this.Background = this.IsClickable ? Brushes.LightGray : Brushes.White;
            }
        }
    }
}
