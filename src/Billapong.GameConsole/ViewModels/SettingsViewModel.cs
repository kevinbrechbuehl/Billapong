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
        /// The save settings command
        /// </summary>
        private DelegateCommand saveSettingsCommand;

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
                return this.GetValue<string>();
            }

            set
            {
                this.SetValue(value);
                this.ValidateSettings();
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
                return this.saveSettingsCommand ?? (this.saveSettingsCommand = new DelegateCommand(this.SaveSettings, () => !this.HasValidationErrors));
            }
        }

        /// <summary>
        /// Validates the settings.
        /// </summary>
        private void ValidateSettings()
        {
            this.ClearAllValidationMessages();

            if (string.IsNullOrWhiteSpace(this.PlayerName))
            {
                this.SetValidationMessage(() => this.PlayerName, "The player name is required");
            }

            this.SaveSettingsCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            if (!this.HasValidationErrors) 
            {
                Settings.Default.PlayerName = this.PlayerName;
                Settings.Default.Save();
                this.NavigateBack();
            }
        }
    }
}
