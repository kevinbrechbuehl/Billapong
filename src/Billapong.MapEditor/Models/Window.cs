using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models
{
    using System.Windows.Media;
    using Core.Client.UI;

    public class Window : NotificationObject
    {
        public Window(int x, int y, bool isChecked)
        {
            this.X = x;
            this.Y = y;
            this.IsChecked = isChecked;
        }
        
        public int X { get; private set; }

        public int Y { get; private set; }

        private Brush background;

        public Brush Background
        {
            get
            {
                return this.background;
            }

            private set
            {
                this.background = value;
                OnPropertyChanged();
            }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
                this.Background = this.isChecked ? Brushes.LightBlue : Brushes.LightGray;
            }
        }
    }
}
