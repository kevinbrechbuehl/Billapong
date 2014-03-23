namespace Billapong.MapEditor.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;
    using Models.Parameters;

    /// <summary>
    /// Game field canvas.
    /// </summary>
    public class GameFieldCanvas : Canvas
    {
        #region Dependency properties

        /// <summary>
        /// The click command property
        /// </summary>
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(GameFieldCanvas), new PropertyMetadata(ClickCommandChanged));

        /// <summary>
        /// Gets or sets the click command.
        /// </summary>
        /// <value>
        /// The click command.
        /// </value>
        public ICommand ClickCommand
        {
            get
            {
                return (ICommand)GetValue(ClickCommandProperty);
            }

            set
            {
                this.SetValue(ClickCommandProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// The canvas has changed, readd the mouse left button event.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void ClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        /// <summary>
        /// Called when the user clicks on the canvas, fire event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (GameFieldCanvas)sender;
            if (canvas.DataContext is GameWindow)
            {
                var args = new GameWindowClickedArgs
                {
                    Point = e.GetPosition((IInputElement)sender),
                    GameWindow = (GameWindow)canvas.DataContext
                };

                if (canvas.ClickCommand.CanExecute(args))
                {
                    canvas.ClickCommand.Execute(args);
                }
            }
        }
    }
}
