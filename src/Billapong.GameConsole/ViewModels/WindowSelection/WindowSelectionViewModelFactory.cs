namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using Billapong.GameConsole.Configuration;
    using Billapong.GameConsole.Models;

    /// <summary>
    /// Factory for instantiating the corresponding window selection view model based on the game type
    /// </summary>
    public class WindowSelectionViewModelFactory
    {
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="map">The map.</param>
        /// <returns>The instance.</returns>
        public static IMainWindowContentViewModel CreateInstance(GameConfiguration.GameType type, Map map)
        {
            switch (type)
            {
                case GameConfiguration.GameType.SinglePlayerTraining:
                    return new SpTrainingWindowSelectionViewModel(map);
                case GameConfiguration.GameType.MultiPlayerGame:
                    return new MpWindowSelectionViewModel(map);
                default:
                    return new SpWindowSelectionViewModel(map);
            }
        }
    }
}
