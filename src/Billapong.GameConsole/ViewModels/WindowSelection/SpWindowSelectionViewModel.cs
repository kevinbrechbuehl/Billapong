namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using Billapong.GameConsole.Models;

    /// <summary>
    /// The window selection view model for a singleplayer game
    /// </summary>
    public class SpWindowSelectionViewModel : WindowSelectionViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpWindowSelectionViewModel"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public SpWindowSelectionViewModel(Map map)
            : base(map)
        {
        }
    }
}
