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
            var callbackClient = new GameConsoleCallbackClient();
            callbackClient.GameStarted += (sender, args) => MessageBox.Show("Started as initiator");

            var client = new GameConsoleServiceClient(callbackClient);
            var gameId = client.OpenGame(this.Map.Id, new[] { this.Map.Windows.First().Id }, Properties.Settings.Default.PlayerName);

            var loadingScreen = new LoadingScreenViewModel("Waiting for opponent...");
            this.SwitchWindowContent(loadingScreen);
        }


    }
}
