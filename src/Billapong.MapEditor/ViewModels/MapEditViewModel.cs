using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.ViewModels
{
    using System.Globalization;
    using System.ServiceModel.Channels;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Input;
    using System.Windows.Media;
    using Contract.Service;
    using Core.Client.UI;
    using Converter;
    using Models;
    using Models.Events;
    using Models.Parameters;
    using Services;

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

        public string MapName
        {
            get
            {
                return this.map.Name;
            }

            set
            {
                this.map.Name = value;
                OnPropertyChanged();
            }
        }

        private bool isWindowHolesSelectionMode;

        public GameWindow[][] GameWindows { get; private set; }

        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.Save);
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

        public MapEditViewModel() : this (new Map())
        {
        }

        public MapEditViewModel(Map map)
        {
            // initialize
            this.callback = new MapEditorCallback();
            this.callback.GeneralDataSaved += this.GeneralDataSaved;
            this.callback.WindowAdded += this.WindowAdded;
            this.callback.WindowRemoved += this.WindowRemoved;
            this.callback.HoleAdded += this.HoleAdded;
            this.callback.HoleRemoved += this.HoleRemoved;
            this.proxy = new MapEditorServiceClient(this.callback);
            this.map = map;

            // register the callback
            if (map.Id > 0)
            {
                this.proxy.RegisterCallback(map.Id); // todo (breck1): nach der ersten änderung am client registrieren
            }

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

        public void GeneralDataSaved(object sender, GeneralDataSavedEventArgs args)
        {
            this.map.Id = args.Id;
            this.MapName = args.Name;
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

        private void Save()
        {
            this.proxy.SaveGeneral(this.map.ToGeneralMapData());
        }

        private void ToggleSelectionMode()
        {
            this.isWindowHolesSelectionMode = !isWindowHolesSelectionMode;
        }

        private void GameWindowClicked(GameWindowClickedArgs args)
        {
            var point = args.Point;
            var gameWindow = args.GameWindow;
            
            if (this.isWindowHolesSelectionMode)
            {
                if (!gameWindow.IsChecked)
                {
                    // todo (breck1): make mvvm conform
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
    }
}
