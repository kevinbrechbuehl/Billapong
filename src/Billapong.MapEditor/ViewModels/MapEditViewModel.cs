using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
    using Core.Client.UI;
    using Models;

    public class MapEditViewModel : ViewModelBase
    {
        private Map map;

        public MapEditViewModel() : this (new Map())
        {
        }

        public MapEditViewModel(Map map)
        {
            this.map = map;
        }
    }
}
