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

        public int NumberOfWindows { get; set; }

        public int NumberOfHoles { get; set; }

        public IList<Contract.Data.Map.Window> Windows { get; set; }
    }
}
