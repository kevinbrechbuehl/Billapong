namespace Billapong.GameConsole.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Windows;
    using System.Windows.Input;

    using Billapong.GameConsole.Views;

    using Contract.Data.Tracing;
    using Contract.Service;
    using Core.Client.Tracing;
    using Service;
    using Billapong.GameConsole.Models;

    public class GameMenuViewModel : UserControlViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenuViewModel"/> class.
        /// </summary>
        public GameMenuViewModel()
        {
            this.WindowHeight = 300;
            this.WindowWidth = 400;
        }

        /// <summary>
        /// Gets the open game window command.
        /// </summary>
        /// <value>
        /// The open game window command.
        /// </value>
        public ICommand OpenGameWindowCommand
        {
            get
            {
                return new DelegateCommand(OpenGameWindow);
            }
        }

        /// <summary>
        /// Gets the open map selection command.
        /// </summary>
        /// <value>
        /// The open map selection command.
        /// </value>
        public ICommand OpenMapSelectionCommand
        {
            get
            {
                return new DelegateCommand(OpenMapSelection);
            }
        }

        /// <summary>
        /// Gets the log command.
        /// </summary>
        /// <value>
        /// The log command.
        /// </value>
        public ICommand LogCommand
        {
            get
            {
                return new DelegateCommand(Log);
            }
        }

        /// <summary>
        /// Gets the get maps command.
        /// </summary>
        /// <value>
        /// The get maps command.
        /// </value>
        public ICommand GetMapsCommand
        {
            get
            {
                return new DelegateCommand(GetMaps);
            }
        }

        /// <summary>
        /// Opens the map selection.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void OpenMapSelection(object parameter)
        {
            var viewModel = new MapSelectionViewModel();
            base.OnWindowContentSwapRequested(new WindowContentSwapRequestedEventArgs(viewModel));
        }

        /// <summary>
        /// Opens the game window.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void OpenGameWindow(object parameter)
        {
            var gameWindow = new GameWindow();
            gameWindow.Show();
        }

        /// <summary>
        /// Test logging method
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void Log(object parameter)
        {
            var exception = new Exception();
            try
            {
                int.Parse("lala");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            
            Tracer.Debug("this is log via tracer");
            Tracer.Info("this is a info via tracer");
            Tracer.Warn("WARNING");
            Tracer.Error("WOHO, error", exception);

            var messages = Tracer.GetLogMessages();
            MessageBox.Show(string.Format("Current number of log messages saved on server: {0}", messages.Count()));
        }

        /// <summary>
        /// Gets the maps.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void GetMaps(object parameter)
        {
            var client = new GameConsoleServiceClient();
            var maps = client.GetMaps().ToList();

            var mapsInfo = new List<string> { "Number of maps: " + maps.Count };

            foreach (var map in maps)
            {
                mapsInfo.Add(string.Format("Name: {0}, Windows: {1}, Holes: {2}", map.Name, map.Windows.Count, map.Windows.Select(window => window.Holes.Count).Sum()));
            }

            MessageBox.Show(string.Join(Environment.NewLine, mapsInfo));
        }
    }
}
