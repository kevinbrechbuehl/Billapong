namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using Billapong.GameConsole.Configuration;
    using Billapong.GameConsole.Models;

    public static class WindowSelectionViewModelFactory
    {
        public static IWindowSelectionViewModel CreateInstance(GameConfiguration.GameType type, Map map)
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
