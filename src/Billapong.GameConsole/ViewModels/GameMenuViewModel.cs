namespace Billapong.GameConsole.ViewModels
{
    using System.Diagnostics;
    using System.IO;
    using System.ServiceModel.Description;
    using System.Windows;
    using Billapong.GameConsole.Properties;
    using Configuration;
    using Core.Client.UI;

    /// <summary>
    /// The game menu view model
    /// </summary>
    public class GameMenuViewModel : MainWindowContentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        public GameMenuViewModel()
        {
            this.WindowHeight = 350;
            this.WindowWidth = 400;
        }

        /// <summary>
        /// Gets the open map selection command.
        /// </summary>
        /// <value>
        /// The open map selection command.
        /// </value>
        public DelegateCommand<string> OpenMapSelectionCommand
        {
            get
            {
                return new DelegateCommand<string>(this.OpenMapSelection);
            }
        }

        /// <summary>
        /// Gets the open game lobby command.
        /// </summary>
        /// <value>
        /// The open game lobby command.
        /// </value>
        public DelegateCommand OpenGameLobbyCommand
        {
            get
            {
                return new DelegateCommand(this.OpenGameLobby);
            }
        }

        /// <summary>
        /// Gets the open settings command.
        /// </summary>
        /// <value>
        /// The open settings command.
        /// </value>
        public DelegateCommand OpenSettingsCommand
        {
            get
            {
                return new DelegateCommand(this.OpenSettings);
            }
        }

        /// <summary>
        /// Gets the start map editor command.
        /// </summary>
        /// <value>
        /// The start map editor command.
        /// </value>
        public DelegateCommand StartMapEditorCommand
        {
            get
            {
                return new DelegateCommand(this.StartMapEditor);
            }
        }

        /// <summary>
        /// Opens the map selection.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void OpenMapSelection(string parameter)
        {
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.PlayerName))
            {
                GameConfiguration.GameType gameType;

                switch (!string.IsNullOrWhiteSpace(parameter) ? parameter : string.Empty)
                {
                    case "training":
                        gameType = GameConfiguration.GameType.SinglePlayerTraining;
                        break;
                    case "multiplayer":
                        gameType = GameConfiguration.GameType.MultiPlayerGame;
                        break;
                    default:
                        gameType = GameConfiguration.GameType.SinglePlayerGame;
                        break;
                }

                var viewModel = new MapSelectionViewModel(gameType);
                this.SwitchWindowContent(viewModel);
            }
            else
            {
                this.OpenSettings();
            }
        }

        /// <summary>
        /// Opens the game lobby.
        /// </summary>
        private void OpenGameLobby()
        {
            var viewModel = new GameLobbyViewModel();
            this.SwitchWindowContent(viewModel);
        }

        /// <summary>
        /// Opens the settings.
        /// </summary>
        private void OpenSettings()
        {
            var viewModel = new SettingsViewModel();
            this.SwitchWindowContent(viewModel);
        }

        /// <summary>
        /// Starts the map editor.
        /// </summary>
        private void StartMapEditor()
        {
            var mapEditorExecutablePath = this.FindMapEditorExecutablePath();
            if (!string.IsNullOrWhiteSpace(mapEditorExecutablePath))
            {
                var startInfo = new ProcessStartInfo(@"..\..\..\Billapong.MapEditor\bin\Debug\Billapong.MapEditor.exe");
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(
                    "Cannot find the map editor executable",
                    Resources.Error,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Finds the map editor executable path.
        /// </summary>
        /// <returns>The executable path or null</returns>
        private string FindMapEditorExecutablePath()
        {
            string path = null;
            const string MapEditorExecutable = "Billapong.MapEditor.exe";

            // Check for the file in the same folder as the current application
            if (File.Exists(MapEditorExecutable))
            {
                path = MapEditorExecutable;
            }
            else
            {
                // Check for the executable within the visual studio solution based on DEBUG and RELEASE build
                var buildFolderType = "Release";

                #if DEBUG
                    buildFolderType = "Debug";
                #endif

                var testPath = string.Format(@"..\..\..\Billapong.MapEditor\bin\{0}\{1}", buildFolderType, MapEditorExecutable);
                if (File.Exists(testPath))
                {
                    path = testPath;
                }
            }

            return path;
        }    
    }
}
