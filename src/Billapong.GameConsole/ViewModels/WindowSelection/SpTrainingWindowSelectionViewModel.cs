namespace Billapong.GameConsole.ViewModels.WindowSelection
{
    using System;
    using System.Windows;
    using Billapong.GameConsole.Models;
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
        /// <param name="properties">The properties.</param>
        protected override void StartGame(object properties)
        {
            var game = new Game();
            game.Init(new Guid(), this.Map, null, true);
            GameManager.Instance.StartGame(game);
        }
    }
}
