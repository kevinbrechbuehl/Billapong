namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Windows;
    using Billapong.GameConsole.Models;

    /// <summary>
    /// The window selection view model for a multiplayer game
    /// </summary>
    public class MpWindowSelectionViewModel : WindowSelectionViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpWindowSelectionViewModel"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public MpWindowSelectionViewModel(Map map)
            : base(map)
        {
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="properties">The properties.</param>
        protected override void StartGame(object properties)
        {
            MessageBox.Show("Nun würden wir auf einen Gegner warten");
        }
    }
}
