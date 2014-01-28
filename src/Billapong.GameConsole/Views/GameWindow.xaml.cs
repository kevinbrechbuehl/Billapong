namespace Billapong.GameConsole.Views
{
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Animation;
    using System.Collections.Concurrent;
    using System.Windows;

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
        }
    }
}
