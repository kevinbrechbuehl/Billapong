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

        public string Name
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public bool IsPlayable
        {
            get
            {
                return this.GetValue<bool>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        public int NumberOfWindows
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        public int NumberOfHoles
        {
            get
            {
                return this.GetValue<int>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        public IList<Contract.Data.Map.Window> Windows { get; set; }
    }
}
