namespace Billapong.GameConsole.Models
{
    using System.Windows;
    using System.Windows.Media;
    using Core.Client.UI;

    /// <summary>
    /// Represents the game ball
    /// </summary>
    public class Ball : NotificationObject
    {
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
                return Configuration.GameConfiguration.BallDiameter;
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
                return Configuration.GameConfiguration.BallDiameter / 2;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Point Position
        {
            get
            {
                return this.GetValue<Point>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get
            {
                return this.GetValue<Color>();
            }

            set
            {
                this.SetValue(value);
            }
        }
    }
}
