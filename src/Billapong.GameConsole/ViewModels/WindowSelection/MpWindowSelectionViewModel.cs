namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System;
    using System.Linq;
    using System.Windows;
    using Billapong.GameConsole.Models;
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
            var client = new GameConsoleServiceClient();
            var gameId = client.OpenGame(this.Map.Id, new long[]{this.Map.Windows.First().Id}, "Pascal");
            MessageBox.Show("Game started with guid: " + gameId);
        }

        protected override void JoinGame(object properties)
        {
            var client = new GameConsoleServiceClient();
            client.JoinGame(new Guid(), "Kevin");
        }
    }
}
