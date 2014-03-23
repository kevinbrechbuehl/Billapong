namespace Billapong.MapEditor.Models
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using Converter;
    using Core.Client.UI;

    /// <summary>
    /// Game window model.
    /// </summary>
    public class GameWindow : NotificationObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="mapWindow">The map window.</param>
        /// <param name="holeDiameter">The hole diameter.</param>
        public GameWindow(int x, int y, Contract.Data.Map.Window mapWindow, double holeDiameter)
        {
            this.X = x;
            this.Y = y;
            this.Holes = new ObservableCollection<Hole>();

            if (mapWindow != null)
            {
                this.Id = mapWindow.Id;
                this.IsChecked = true;
                foreach (var hole in mapWindow.Holes)
                {
                    this.Holes.Add(hole.ToEntity(holeDiameter));
                }
            }
            else
            {
                this.IsChecked = false;
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets the x coords.
        /// </summary>
        /// <value>
        /// The x coords.
        /// </value>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y coords.
        /// </summary>
        /// <value>
        /// The y coords.
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
                return this.GetValue<Brush>();
            }

            private set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current window is checked/active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if current window is checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsChecked
        {
            get
            {
                return this.GetValue<bool>();
            }

            set
            {
                this.SetValue(value);
                this.Background = this.GetValue<bool>() ? Brushes.LightBlue : Brushes.LightGray;
            }
        }

        /// <summary>
        /// Gets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public ObservableCollection<Hole> Holes { get; private set; }
    }
}
