namespace Billapong.MapEditor.Models
{
    using Core.Client.UI;

    public class Hole : NotificationObject
    {
        public Hole(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }
    }
}
