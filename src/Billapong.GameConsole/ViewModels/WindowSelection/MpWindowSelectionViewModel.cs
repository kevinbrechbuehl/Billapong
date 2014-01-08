namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Windows;
    using Billapong.GameConsole.Models;

    public class MpWindowSelectionViewModel : WindowSelectionViewModelBase
    {
        public MpWindowSelectionViewModel(Map map)
            : base(map)
        {
        }

        protected override void StartGame(object properties)
        {
            MessageBox.Show("Nun würden wir auf einen Gegner warten");
        }
    }
}
