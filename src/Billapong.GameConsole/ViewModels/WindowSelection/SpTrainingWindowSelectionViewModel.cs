namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Windows;
    using Billapong.GameConsole.Models;

    public class SpTrainingWindowSelectionViewModel : WindowSelectionViewModelBase
    {
        public SpTrainingWindowSelectionViewModel(Map map) 
            : base(map)
        {
        }

        protected override void StartGame(object properties)
        {
            MessageBox.Show("Nun würde das Training starten");
        }
    }
}
