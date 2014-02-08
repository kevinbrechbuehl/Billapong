namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System;
    using System.Windows;
    using Billapong.GameConsole.Models;
    using Configuration;
    using Game;

    /// <summary>
    /// The window selection view model for a singleplayer training
    /// </summary>
    public class SpTrainingWindowSelectionViewModel : WindowSelectionViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpTrainingWindowSelectionViewModel"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public SpTrainingWindowSelectionViewModel(Map map) 
            : base(map)
        {
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        protected override void StartGame()
        {
            var game = new Game();
            game.Init(new Guid(), this.Map, null, true, GameConfiguration.GameType.SinglePlayerTraining);
            GameManager.Current.StartGame(game);
        }
    }
}
