namespace Billapong.GameConsole.ViewModels
{
    using Billapong.GameConsole.Properties;

    using Core.Client.UI;

    /// <summary>
    /// Respresents the logic of the settings view
    /// </summary>
    public class SettingsViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// The player name
        /// </summary>
        private string playerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel()
        {
            this.WindowHeight = 350;
            this.WindowWidth = 400;

            this.PlayerName = Settings.Default.PlayerName;
            this.BackButtonContent = Resources.BackToMenu;
        }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName
        {
            get
            {
                return this.playerName;
            }

            set
            {
                this.playerName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the save settings command.
        /// </summary>
        /// <value>
        /// The save settings command.
        /// </value>
        public DelegateCommand SaveSettingsCommand
        {
            get
            {
                return new DelegateCommand(this.SaveSettings);
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            Settings.Default.PlayerName = this.PlayerName;
            Settings.Default.Save();

            this.NavigateBack();
        }
    }
}
