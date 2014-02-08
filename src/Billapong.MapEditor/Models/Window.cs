using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models
{
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Converter;
    using Core.Client.UI;

    public class Window : NotificationObject
    {
        public Window(int x, int y, Contract.Data.Map.Window mapWindow, double holeDiameter)
        {
            this.X = x;
            this.Y = y;
            this.Holes = new ObservableCollection<Hole>();

            if (mapWindow != null)
            {
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

        public ObservableCollection<Hole> Holes { get; private set; }
    }
}
