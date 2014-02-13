namespace Billapong.GameConsole.Models
{
    using System.Windows;
    using Core.Client.UI;

    public class Ball : NotificationObject
    {
        /// <summary>
        /// The position
        /// </summary>
        private Point position;

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
                return this.position;
            }
            set
            {
                this.position = value;
                OnPropertyChanged();
            }
        }
    }
}
