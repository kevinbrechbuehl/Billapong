namespace Billapong.GameConsole.Models
{
    using System.Windows;
    using Animation;
    using Core.Client.UI;

    public class Ball : NotificationObject
    {
        public double Radius
        {
            get
            {
                return Configuration.GameConfiguration.BallDiameter / 2;
            }
        }

        private Point position;

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

        private BallAnimationTask pointAnimation;

        public BallAnimationTask PointAnimation
        {
            get
            {
                return this.pointAnimation;
            }
            set
            {
                this.pointAnimation = value;
                OnPropertyChanged();
            }
        }

    }
}
