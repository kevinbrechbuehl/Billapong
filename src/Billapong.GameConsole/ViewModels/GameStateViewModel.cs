namespace Billapong.GameConsole.ViewModels
{
    using System.Collections.ObjectModel;
    using Core.Client.UI;
    using Game;
    using Models;

    public class GameStateViewModel : MainWindowContentViewModelBase
    {
        public GameStateViewModel(Game game)
        {
            this.WindowHeight = 160;
            this.WindowWidth = 300;
            this.Game = game;

            this.Players = new ObservableCollection<Player>();
        }

        public DelegateCommand CancelGameCommand
        {
            get
            {
                return new DelegateCommand(GameManager.Current.CancelGame);
            }
        }

        public ObservableCollection<Player> Players { get; private set; }

        public Game Game { get; private set; }

        public void StartGame()
        {
            this.Players.Add(GameManager.Current.CurrentGame.LocalPlayer);

            if (GameManager.Current.CurrentGame.Opponent != null)
            {
                this.Players.Add(GameManager.Current.CurrentGame.Opponent);
            }
        }
    }
}
