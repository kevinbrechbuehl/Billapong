namespace Billapong.GameConsole.Views
{
    using Animation;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Shapes;
    using ViewModels;

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// The ball animation queue
        /// </summary>
        public ConcurrentQueue<BallAnimationTask> BallAnimationTaskQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            this.InitializeComponent();

            //this.holeDiameter = this.MapCanvas.Width/GridSize;
            //this.ballRadius = this.holeDiameter*0.667/2;
        }

        public void InitializeHoles()
        {
            var viewModel = this.DataContext as GameWindowViewModel;
        }
    }
}
