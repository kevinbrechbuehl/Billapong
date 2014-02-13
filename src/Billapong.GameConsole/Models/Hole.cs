namespace Billapong.GameConsole.Models
{
    using System;
    using System.Windows;
    using Configuration;
    using Core.Client.UI;

    /// <summary>
    /// Represents a hole within a window
    /// </summary>
    public class Hole : NotificationObject
    {
        /// <summary>
        /// The left position
        /// </summary>
        private double left;

        /// <summary>
        /// The top position
        /// </summary>
        private double top;

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

        public double Diameter
        {
            get
            {
                return GameConfiguration.HoleDiameter;
            }
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double Radius
        {
            get
            {
                return GameConfiguration.HoleDiameter / 2;
            }
        }

        /// <summary>
        /// Gets or sets the left position.
        /// </summary>
        /// <value>
        /// The left position.
        /// </value>
        public double Left
        {
            get
            {
                if (Math.Abs(this.left) < 0.001)
                {
                    this.left = this.Diameter*this.X;
                }

                return this.left;
            }

            set
            {
                this.left = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the top position.
        /// </summary>
        /// <value>
        /// The top position.
        /// </value>
        public double Top
        {
            get
            {
                if (Math.Abs(this.top) < 0.001)
                {
                    this.top = this.Diameter * this.Y;
                }

                return this.top;
            }

            set
            {
                this.top = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the center position of the hole based on the Left and Top properties
        /// </summary>
        /// <value>
        /// The center position.
        /// </value>
        public Point CenterPosition
        {
            get
            {
                return new Point(this.Left + this.Radius, this.top + this.Radius);
            }
        }
    }
}
