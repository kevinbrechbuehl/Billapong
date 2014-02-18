namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Linq;
    using System.Windows;
    using Configuration;
    using Models;
    using Service;

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
        protected override void StartGame()
        {
            var loadingScreen = new LoadingScreenViewModel("Waiting for opponent...", GameConfiguration.GameType.MultiPlayerGame, true);
            GameConsoleContext.Current.GameConsoleCallback.GameStarted += loadingScreen.StartGame;
            var gameId = GameConsoleContext.Current.GameConsoleServiceClient.OpenGame(this.Map.Id, this.Map.Windows.Where(window => window.IsOwnWindow).Select(window => window.Id), Properties.Settings.Default.PlayerName);

            this.SwitchWindowContent(loadingScreen);
        }


    }
}
