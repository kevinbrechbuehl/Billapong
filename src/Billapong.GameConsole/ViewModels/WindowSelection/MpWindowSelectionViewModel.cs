namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System.Linq;
    using System.Windows;
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
        /// <param name="properties">The properties.</param>
        protected override void StartGame(object properties)
        {
            var loadingScreen = new LoadingScreenViewModel("Waiting for opponent...");
            var callback = new GameConsoleCallback();
            callback.GameStarted += loadingScreen.StartGame;

            var client = new GameConsoleServiceClient(callback);
            var gameId = client.OpenGame(this.Map.Id, this.Map.Windows.Where(window => window.IsOwnWindow).Select(window => window.Id), Properties.Settings.Default.PlayerName);

            this.SwitchWindowContent(loadingScreen);
        }


    }
}
