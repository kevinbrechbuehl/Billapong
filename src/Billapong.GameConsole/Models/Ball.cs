namespace Billapong.GameConsole.Models
{
    using Core.Client.UI;

    public class Ball : NotificationObject
    {
        public double Radius
        {
            get
            {
                return Configuration.GameConfiguration.BallRadius;
            }
        }

        private int left;

        public int Left
        {
            get
            {
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
