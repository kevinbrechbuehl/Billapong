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
            this.proxy = new MapEditorServiceClient(this.callback);
            this.map = map;

            // register the callback
            if (map.Id > 0)
            {
                this.proxy.RegisterCallback(map.Id);
            }

            // get the maps config and display it
            var config = this.proxy.GetMapConfiguration();
            var windows = new GameWindow[config.WindowRows][];
            this.gridSize = GameWindowSize / config.HoleGrid;
            this.HoleDiameter = GameWindowSize/config.HoleGrid;

            for (var i = 0; i < windows.Length; i++)
            {
                windows[i] = new GameWindow[config.WindowCols];
                for (var j=0; j<windows[i].Length; j++)
                {
                    windows[i][j] = new GameWindow(j, i, this.map.Windows.FirstOrDefault(item => item.X == i && item.Y == j), this.HoleDiameter);
                }
            }

            this.GameWindows = windows;
        }

        public void GeneralDataSaved(object sender, GeneralDataSavedEventArgs args)
        {
            this.map.Id = args.Id;
            this.MapName = args.Name;
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
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                var coordX = (int)(point.X / this.gridSize);
                var coordY = (int)(point.Y / this.gridSize);

                var holeOnField = gameWindow.Holes.FirstOrDefault(hole => hole.X == coordX && hole.Y == coordY);
                if (holeOnField != null)
                {
                    gameWindow.Holes.Remove(holeOnField);
                }
                else
                {
                    gameWindow.Holes.Add(new Hole { X = coordX, Y = coordY, Diameter = this.HoleDiameter });   
                }
            }
            else
            {
                gameWindow.IsChecked = !gameWindow.IsChecked;
                if (!gameWindow.IsChecked)
                {
                    gameWindow.Holes.Clear();
                }
            }
        }
    }
}
