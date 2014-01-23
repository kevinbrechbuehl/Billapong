namespace Billapong.GameConsole.ViewModels
{
    using Core.Client.UI;

    public class SettingsViewModel : MainWindowContentViewModelBase
    {
        private string playerName;

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }

            set
            {
                this.playerName = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SaveSettingsCommand
        {
            get
            {
                return new DelegateCommand(this.SaveSettings);
            }
        }


        public SettingsViewModel()
        {
            this.WindowHeight = 350;
            this.WindowWidth = 400;

            this.PlayerName = Properties.Settings.Default.PlayerName;
            this.BackButtonContent = "Back to menu";
        }

        public void SaveSettings(object properties)
        {
            Properties.Settings.Default.PlayerName = this.PlayerName;
            Properties.Settings.Default.Save();
        }
    }
}
