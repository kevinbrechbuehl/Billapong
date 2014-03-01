using System.Linq;

namespace Billapong.MapEditor.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Navigation;
    using Converter;
    using Core.Client.UI;
    using Models;
    using Models.Events;
    using Models.Parameters;
    using Services;
    using System.Windows;

    public class MapEditViewModel : ViewModelBase
    {
        public const double GameWindowSize = 200;

        private readonly Map map;

        private readonly MapEditorServiceClient proxy;

        private readonly MapEditorCallback callback;

        private readonly double gridSize;

        private double holeDiameter;

        public double HoleDiameter
        {
            get
            {
                return this.holeDiameter;
            }

            set
            {
                this.holeDiameter = value;
                OnPropertyChanged();
            }
        }

        private bool isPlayable;

        public bool IsPlayable
        {
            get
            {
                return this.isPlayable;
            }

            set
            {
                this.isPlayable = value;
                this.map.IsPlayable = value;
                OnPropertyChanged();
            }
        }

        private string mapName;

        public string MapName
        {
            get
            {
                return this.mapName;
            }

            set
            {
                this.mapName = value;
                this.map.Name = value;
                OnPropertyChanged();
            }
        }

        private bool isWindowHolesSelectionMode;

        public GameWindow[][] GameWindows { get; private set; }

        public DelegateCommand SaveNameCommand
        {
            get
            {
                return new DelegateCommand(this.SaveName);
            }
        }

        public DelegateCommand ToggleIsPlayableCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleIsPlayable);
            }
        }

        public DelegateCommand ToggleSelectionModeCommand
        {
            get
            {
                return new DelegateCommand(this.ToggleSelectionMode);
            }
        }

        public DelegateCommand<GameWindowClickedArgs> GameWindowClickedCommand
        {
            get
            {
                return new DelegateCommand<GameWindowClickedArgs>(this.GameWindowClicked);
            }
        }

        public MapEditViewModel(Map map = null)
        {
            // initialize
            this.callback = new MapEditorCallback();
            this.callback.NameUpdated += this.NameUpdated;
            this.callback.IsPlayableUpdated += this.IsPlayableUpdated;
            this.callback.WindowAdded += this.WindowAdded;
            this.callback.WindowRemoved += this.WindowRemoved;
            this.callback.HoleAdded += this.HoleAdded;
            this.callback.HoleRemoved += this.HoleRemoved;
            this.proxy = new MapEditorServiceClient(this.callback);
            
            // validate map
            if (map == null)
            {
                map = this.proxy.CreateMap().ToEntity();
            }

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
            this.HoleDiameter = GameWindowSize/config.HoleGrid;

            for (var row = 0; row < windows.Length; row++)
            {
                windows[row] = new GameWindow[config.WindowCols];
                for (var col=0; col<windows[row].Length; col++)
                {
                    var mapWindow = this.map.Windows != null
                        ? this.map.Windows.FirstOrDefault(item => item.X == col && item.Y == row)
                        : null;
                    windows[row][col] = new GameWindow(col, row, mapWindow, this.HoleDiameter);
                }
            }

            this.GameWindows = windows;
        }

        public override void CloseCallback()
        {
            this.proxy.UnregisterCallback(this.map.Id);
        }

        private void NameUpdated(object sender, UpdateNameEventArgs args)
        {
            this.MapName = args.Name;
        }

        private void IsPlayableUpdated(object sender, UpdateIsPlayableEventArgs args)
        {
            this.IsPlayable = args.IsPlayable;
        }

        private void WindowAdded(object sender, GameWindowEventArgs args)
        {
            var gameWindow = this.GameWindows[args.Y][args.X];
            gameWindow.IsChecked = true;
            gameWindow.Id = args.Id;
        }

        private void WindowRemoved(object sender, GameWindowEventArgs args)
        {
            var gameWindow = this.GameWindows[args.Y][args.X];
            gameWindow.IsChecked = false;
            gameWindow.Id = 0;
            gameWindow.Holes.Clear();
        }

        private void HoleAdded(object sender, GameHoleClickedEventArgs args)
        {
            var gameWindow = this.GameWindows[args.WindowY][args.WindowX];
            gameWindow.Holes.Add(new Hole { Id = args.HoleId, X = args.HoleX, Y = args.HoleY, Diameter = this.HoleDiameter });
        }

        private void HoleRemoved(object sender, GameHoleClickedEventArgs args)
        {
            var gameWindow = this.GameWindows[args.WindowY][args.WindowX];
            var hole = gameWindow.Holes.FirstOrDefault(gameHole => gameHole.Id == args.HoleId);
            if (hole != null)
            {
                gameWindow.Holes.Remove(hole);
            }
        }

        private void ToggleSelectionMode()
        {
            this.isWindowHolesSelectionMode = !isWindowHolesSelectionMode;
        }

        private void SaveName()
        {
            this.proxy.UpdateName(this.map.Id, this.MapName);
        }

        private void ToggleIsPlayable()
        {
            if (!this.IsPlayable || this.IsMapPlayable())
            {
                this.proxy.UpdateIsPlayable(this.map.Id, this.IsPlayable);   
            }
            else
            {
                MessageBox.Show("Map is currently not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.IsPlayable = false;
            }
        }

        private void GameWindowClicked(GameWindowClickedArgs args)
        {
            var point = args.Point;
            var gameWindow = args.GameWindow;
            
            if (this.isWindowHolesSelectionMode)
            {
                if (!gameWindow.IsChecked)
                {
                    MessageBox.Show("You can't add holes here, please activate the game window first", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                var coordX = (int)(point.X / this.gridSize);
                var coordY = (int)(point.Y / this.gridSize);

                var holeOnField = gameWindow.Holes.FirstOrDefault(hole => hole.X == coordX && hole.Y == coordY);
                if (holeOnField != null)
                {
                    this.proxy.RemoveHole(this.map.Id, gameWindow.Id, holeOnField.Id);
                }
                else
                {
                    this.proxy.AddHole(this.map.Id, gameWindow.Id, coordX, coordY);
                }
            }
            else
            {
                if (gameWindow.IsChecked)
                {
                    this.proxy.RemoveWindow(this.map.Id, gameWindow.Id);
                }
                else
                {
                    this.proxy.AddWindow(this.map.Id, gameWindow.X, gameWindow.Y);
                }
            }
        }

        private bool IsMapPlayable()
        {
            var graph = this.GetActiveGraph();
            return graph.Any() && this.GameWindows.All(windows => windows.Where(window => window.IsChecked).All(window => graph.Contains(window.Id)));
        }

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
