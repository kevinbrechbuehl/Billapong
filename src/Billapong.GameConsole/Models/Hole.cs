namespace Billapong.GameConsole.Models
{
    using Configuration;
    using Core.Client.UI;

    /// <summary>
    /// Represents a hole within a window
    /// </summary>
    public class Hole : NotificationObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value> 
        public int Y { get; set; }

        public int Diameter
        {
            get
            {
                return GameConfiguration.HoleDiameter;
            }
        }

        private int left;

        public int Left
        {
            get
            {
                if (left == 0)
                {
                    this.left = this.Diameter*this.X;
                }

                return left;
            }

            set
            {
                this.left = value;
                OnPropertyChanged();
            }
        }

        private int top;

        public int Top
        {
            get
            {
                if (top == 0)
                {
                    this.top = this.Diameter * this.Y;
                }

                return top;
            }

            set
            {
                this.top = value;
                OnPropertyChanged();
            }
        }
    }
}
