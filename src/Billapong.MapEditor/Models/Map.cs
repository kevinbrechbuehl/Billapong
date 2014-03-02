using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Models
{
    using Core.Client.UI;

    public class Map : NotificationObject
    {
        public long Id { get; set; }

        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }

        private bool isPlayable;
        public bool IsPlayable
        {
            get
            {
                return this.isPlayable;
            }

            set
            {
                this.isPlayable = value;
                OnPropertyChanged();
            }
        }

        private int numberOfWindows;

        public int NumberOfWindows
        {
            get
            {
                return this.numberOfWindows;
            }
            set
            {
                this.numberOfWindows = value;
                this.OnPropertyChanged();
            }
        }

        private int numberOfHoles;

        public int NumberOfHoles
        {
            get
            {
                return this.numberOfHoles;
            }
            set
            {
                this.numberOfHoles = value;
                this.OnPropertyChanged();
            }
        }

        public IList<Contract.Data.Map.Window> Windows { get; set; }
    }
}
