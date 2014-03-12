namespace Billapong.GameConsole.Models
{
    using System.Windows;
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
        public int X 
        {    
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
                this.Left = this.Diameter * value;
            }
        }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value> 
        public int Y 
        {
            get
            {
                return this.GetValue<int>();
            }

            set
            {
                this.SetValue(value);
                this.Top = this.Diameter * value;
            }
        }

        /// <summary>
        /// Gets the diameter.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
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
                return this.GetValue<double>();
            }

            set
            {
                this.SetValue(value);
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
                return this.GetValue<double>();
            }

            set
            {
                this.SetValue(value);
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
                return new Point(this.Left + this.Radius, this.Top + this.Radius);
            }
        }
    }
}
