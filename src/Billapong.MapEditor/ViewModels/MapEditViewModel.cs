namespace Billapong.MapEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Windows;
    using Billapong.Contract.Exceptions;
    using Billapong.Core.Client.Exceptions;
    using Billapong.Core.Client.Tracing;
    using Billapong.MapEditor.Properties;
    using Core.Client.UI;
    using Models;
    using Models.Events;
    using Models.Parameters;
    using Services;
    using Hole = Billapong.MapEditor.Models.Hole;
    using Map = Billapong.MapEditor.Models.Map;

    /// <summary>
    /// Map editing view model.
    /// </summary>
    public class MapEditViewModel : ViewModelBase
    {
        /// <summary>
        /// The game window size
        /// </summary>
        public const double GameWindowSize = 200;

        /// <summary>
        /// The map
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// The map editor service proxy
        /// </summary>
        private readonly MapEditorServiceClient proxy;

        /// <summary>
        /// The map editor callback instance
        /// </summary>
        private readonly MapEditorCallback callback;

        /// <summary>
        /// The grid size
        /// </summary>
        private readonly double gridSize;

        /// <summary>
        /// Boolean value if window holes are current selection modes
        /// </summary>
        private bool isWindowHolesSelectionMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditViewModel"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="sessionId">The session identifier.</param>
        public MapEditViewModel(Map map, Guid sessionId)
        {
            // Trace
            Tracer.Debug(string.Format("MapEditViewModel :: Calling new MapEditViewModel() with map id '{0}'", map.Id));

            try
            {
                // initialize
                this.callback = new MapEditorCallback();
                this.callback.NameUpdated += this.NameUpdated;
                this.callback.IsPlayableUpdated += this.IsPlayableUpdated;
                this.callback.WindowAdded += this.WindowAdded;
                this.callback.WindowRemoved += this.WindowRemoved;
                this.callback.HoleAdded += this.HoleAdded;
                this.callback.HoleRemoved += this.HoleRemoved;
                this.proxy = new MapEditorServiceClient(this.callback, sessionId);

                // set map properties
                this.map = map;
                this.IsPlayable = map.IsPlayable;
                this.MapName = map.Name;

                // register the callback
                this.proxy.RegisterCallback(map.Id);

                // get the maps config and display it
                var config = this.proxy.GetMapConfiguration();
                var windows = new GameWindow[config.WindowRows][];
                this.gridSize = GameWindowSize / config.HoleGrid;
                this.HoleDiameter = GameWindowSize / config.HoleGrid;

                for (var row = 0; row < windows.Length; row++)
                {
                    windows[row] = new GameWindow[config.WindowCols];
                    for (var col = 0; col < windows[row].Length; col++)
                    {
                        var mapWindow = this.map.Windows != null
                            ? this.map.Windows.FirstOrDefault(item => item.X == col && item.Y == row)
                            : null;
                        windows[row][col] = new GameWindow(col, row, mapWindow, this.HoleDiameter);
                    }
                }

                this.GameWindows = windows;
            }
            catch (FaultException<CallbackNotValidException>)
            {
                this.HandleInvalidCallback();
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }
        }

        /// <summary>
        /// Gets or sets the hole diameter.
        /// </summary>
        /// <value>
        /// The hole diameter.
        /// </value>
        public double HoleDiameter
        {
            get
            {
                return this.GetValue<double>();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the map is playable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the map is playable; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayable
        {
            get
            {
                return this.GetValue<bool>();
            }

            set
            {
                this.map.IsPlayable = value;
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        public string MapName
        {
            get
            {
                return this.GetValue<string>();
            }

            set
            {
                this.map.Name = value;
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the game windows.
        /// </summary>
        /// <value>
        /// The game windows.
        /// </value>
        public GameWindow[][] GameWindows { get; private set; }

        /// <summary>
        /// Gets the save name command.
        /// </summary>
        /// <value>
        /// The save name command.
        /// </value>
        public DelegateCommand SaveNameCommand
        {
            get
            {
                return new DelegateCommand(this.SaveName);
            }
        }

        /// <summary>
        /// Gets the toggle is playable command.
        /// </summary>
        /// <value>
        /// The toggle is playable command.
        /// </value>
        public DelegateCommand ToggleIsPlayableCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleIsPlayable);
            }
        }

        /// <summary>
        /// Gets the toggle selection mode command.
        /// </summary>
        /// <value>
        /// The toggle selection mode command.
        /// </value>
        public DelegateCommand ToggleSelectionModeCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleSelectionMode);
            }
        }

        /// <summary>
        /// Gets the game window clicked command.
        /// </summary>
        /// <value>
        /// The game window clicked command.
        /// </value>
        public DelegateCommand<GameWindowClickedArgs> GameWindowClickedCommand
        {
            get
            {
                return new DelegateCommand<GameWindowClickedArgs>(this.GameWindowClicked);
            }
        }

        /// <summary>
        /// Gets called when the connected view closes
        /// </summary>
        public override async void CloseCallback()
        {
            await Tracer.Debug("MapEditViewModel :: Close callback retrieved");

            try
            {
                this.proxy.UnregisterCallback(this.map.Id);
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex, true);
            }
        }

        /// <summary>
        /// Updates the name.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="UpdateNameEventArgs"/> instance containing the event data.</param>
        private void NameUpdated(object sender, UpdateNameEventArgs args)
        {
            this.MapName = args.Name;
        }

        /// <summary>
        /// Updates the is playable flag.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="UpdateIsPlayableEventArgs"/> instance containing the event data.</param>
        private void IsPlayableUpdated(object sender, UpdateIsPlayableEventArgs args)
        {
            this.IsPlayable = args.IsPlayable;
        }

        /// <summary>
        /// Window bas been added
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameWindowEventArgs"/> instance containing the event data.</param>
        private void WindowAdded(object sender, GameWindowEventArgs args)
        {
            var gameWindow = this.GameWindows[args.Y][args.X];
            gameWindow.IsChecked = true;
            gameWindow.Id = args.Id;

            // adapt map model
            this.map.NumberOfWindows++;
            var window = new Contract.Data.Map.Window
                             {
                                 Id = args.Id,
                                 X = args.X,
                                 Y = args.Y,
                                 Holes = new List<Contract.Data.Map.Hole>()
                             };
            this.map.Windows.Add(window);
        }

        /// <summary>
        /// Window has been removed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameWindowEventArgs"/> instance containing the event data.</param>
        private void WindowRemoved(object sender, GameWindowEventArgs args)
        {
            var gameWindow = this.GameWindows[args.Y][args.X];
            gameWindow.IsChecked = false;
            gameWindow.Id = 0;
            gameWindow.Holes.Clear();

            // adapt map model
            this.map.NumberOfWindows--;
            var window = this.map.Windows.FirstOrDefault(w => w.Id == args.Id);
            if (window != null)
            {
                this.map.NumberOfHoles -= window.Holes.Count;
                this.map.Windows.Remove(window);
            }
        }

        /// <summary>
        /// Hole has been added.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameHoleClickedEventArgs"/> instance containing the event data.</param>
        private void HoleAdded(object sender, GameHoleClickedEventArgs args)
        {
            var gameWindow = this.GameWindows[args.WindowY][args.WindowX];
            gameWindow.Holes.Add(new Hole { Id = args.HoleId, X = args.HoleX, Y = args.HoleY, Diameter = this.HoleDiameter });

            // adapt map model
            this.map.NumberOfHoles++;
            var window = this.map.Windows.FirstOrDefault(w => w.X == args.WindowX && w.Y == args.WindowY);
            if (window != null)
            {
                var hole = new Contract.Data.Map.Hole { Id = args.HoleId, X = args.HoleX, Y = args.HoleY };
                window.Holes = window.Holes.ToList(); // this is necessary to convert the collection to a list...
                window.Holes.Add(hole);
            }
        }

        /// <summary>
        /// Hole has been removed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameHoleClickedEventArgs"/> instance containing the event data.</param>
        private void HoleRemoved(object sender, GameHoleClickedEventArgs args)
        {
            var gameWindow = this.GameWindows[args.WindowY][args.WindowX];
            var gameHole = gameWindow.Holes.FirstOrDefault(h => h.Id == args.HoleId);
            if (gameHole != null)
            {
                gameWindow.Holes.Remove(gameHole);
            }

            // adapt map model
            this.map.NumberOfHoles--;
            var window = this.map.Windows.FirstOrDefault(w => w.X == args.WindowX && w.Y == args.WindowY);
            if (window != null)
            {
                var hole = window.Holes.FirstOrDefault(h => h.Id == args.HoleId);
                if (hole != null)
                {
                    window.Holes = window.Holes.ToList(); // this is necessary to convert the collection to a list...
                    window.Holes.Remove(hole);
                }
            }
        }

        /// <summary>
        /// Toggles the selection mode.
        /// </summary>
        private void ToggleSelectionMode()
        {
            this.isWindowHolesSelectionMode = !this.isWindowHolesSelectionMode;
        }

        /// <summary>
        /// Saves the name to the server.
        /// </summary>
        private async void SaveName()
        {
            await Tracer.Info(string.Format("MapEditViewModel :: Call SaveName() and update name of map id '{0}' to '{1}", this.map.Id, this.MapName));
            
            try
            {
                await this.proxy.UpdateNameAsync(this.map.Id, this.MapName);
            }
            catch (FaultException<CallbackNotValidException>)
            {
                this.HandleInvalidCallback();
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            } 
        }

        /// <summary>
        /// Toggles the is playable flag on the server.
        /// </summary>
        private async void ToggleIsPlayable()
        {
            if (!this.IsPlayable || this.IsMapPlayable())
            {
                await Tracer.Info(string.Format("MapEditViewModel :: Call ToggleIsPlayable() and set to {0}", this.IsPlayable));
                
                try
                {
                    await this.proxy.UpdateIsPlayableAsync(this.map.Id, this.IsPlayable);
                }
                catch (FaultException<CallbackNotValidException>)
                {
                    this.HandleInvalidCallback();
                }
                catch (ServerUnavailableException ex)
                {
                    this.HandleServerException(ex);
                }   
            }
            else
            {
                await Tracer.Debug("MapEditViewModel :: ToggleIsPlayable() :: Map is currently not playable");
                MessageBox.Show(string.Format(Resources.MapIsNotPlayable, Environment.NewLine), Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.IsPlayable = false;
            }
        }

        /// <summary>
        /// Handle the game window click event (add/remove window/hole).
        /// </summary>
        /// <param name="args">The arguments.</param>
        private async void GameWindowClicked(GameWindowClickedArgs args)
        {
            var point = args.Point;
            var gameWindow = args.GameWindow;

            try
            {
                if (this.isWindowHolesSelectionMode)
                {
                    if (!gameWindow.IsChecked)
                    {
                        MessageBox.Show(
                            Resources.CannotAddHoles,
                            Resources.ErrorTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return;
                    }

                    var coordX = (int)(point.X / this.gridSize);
                    var coordY = (int)(point.Y / this.gridSize);

                    var holeOnField = gameWindow.Holes.FirstOrDefault(hole => hole.X == coordX && hole.Y == coordY);
                    if (holeOnField != null)
                    {
                        await Tracer.Info(
                            string.Format(
                                "MapEditViewModel :: Remove hole '{0}' on window '{1}' on map '{2}'",
                                holeOnField.Id,
                                gameWindow.Id,
                                this.map.Id));
                        await this.proxy.RemoveHoleAsync(this.map.Id, gameWindow.Id, holeOnField.Id);
                    }
                    else
                    {
                        await Tracer.Info(
                            string.Format(
                                "MapEditViewModel :: Add hole on coords '{0}/{1}' to window '{2}' on map '{3}'",
                                coordX,
                                coordY,
                                gameWindow.Id,
                                this.map.Id));
                        await this.proxy.AddHoleAsync(this.map.Id, gameWindow.Id, coordX, coordY);
                    }
                }
                else
                {
                    if (gameWindow.IsChecked)
                    {
                        await Tracer.Info(
                            string.Format(
                                "MapEditViewModel :: Remove window '{0}' from map '{1}'",
                                gameWindow.Id,
                                this.map.Id));
                        await this.proxy.RemoveWindowAsync(this.map.Id, gameWindow.Id);
                    }
                    else
                    {
                        await Tracer.Info(
                            string.Format(
                                "MapEditViewModel :: Add windows on coords '{0}/{1}' to map '{2}",
                                gameWindow.X,
                                gameWindow.Y,
                                this.map.Id));
                        await this.proxy.AddWindowAsync(this.map.Id, gameWindow.X, gameWindow.Y);
                    }
                }
            }
            catch (FaultException<CallbackNotValidException>)
            {
                this.HandleInvalidCallback();
            }
            catch (ServerUnavailableException ex)
            {
                this.HandleServerException(ex);
            }
        }

        /// <summary>
        /// Handles if callback gets invalid.
        /// </summary>
        private void HandleInvalidCallback()
        {
            MessageBox.Show(Resources.InvalidCallback, Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            this.WindowManager.Close(this);
        }

        /// <summary>
        /// Determines whether the curent map configuration if valid.
        /// </summary>
        /// <returns>Boolean value if the map config is valid or not</returns>
        private bool IsMapPlayable()
        {
            var graph = this.GetActiveGraph();
            return graph.Any()
                && this.map.NumberOfHoles > 0
                && this.GameWindows.All(windows => windows.Where(window => window.IsChecked).All(window => graph.Contains(window.Id)));
        }

        /// <summary>
        /// Gets the graph with all active windows.
        /// </summary>
        /// <returns>List with ative windows based on the neighbors graph</returns>
        private IList<long> GetActiveGraph()
        {
            for (var row = 0; row < this.GameWindows.Length; row++)
            {
                for (var col = 0; col < this.GameWindows[row].Length; col++)
                {
                    if (!this.GameWindows[row][col].IsChecked) continue;

                    return this.GetActiveGraph(new List<long>(), row, col);
                }
            }

            return Enumerable.Empty<long>().ToList();
        }

        /// <summary>
        /// Gets the active windows graph.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <returns>List with ative windows based on the neighbors graph</returns>
        private IList<long> GetActiveGraph(IList<long> graph, int row, int col)
        {
            graph.Add(this.GameWindows[row][col].Id);
            
            // neighbor above is active
            if (row > 0 && this.GameWindows[row - 1][col].IsChecked && !graph.Contains(this.GameWindows[row - 1][col].Id))
            {
                graph = this.GetActiveGraph(graph, row - 1, col);
            }

            // neightbor below is active
            if (row + 1 < this.GameWindows.Length && this.GameWindows[row + 1][col].IsChecked && !graph.Contains(this.GameWindows[row + 1][col].Id))
            {
                graph = this.GetActiveGraph(graph, row + 1, col);
            }

            // neightbor to the left is active
            if (col > 0 && this.GameWindows[row][col - 1].IsChecked && !graph.Contains(this.GameWindows[row][col - 1].Id))
            {
                graph = this.GetActiveGraph(graph, row, col - 1);
            }

            // neightbor to the right is active
            if (col + 1 < this.GameWindows[row].Length && this.GameWindows[row][col + 1].IsChecked && !graph.Contains(this.GameWindows[row][col + 1].Id))
            {
                graph = this.GetActiveGraph(graph, row, col + 1);
            }

            return graph;
        }
    }
}
