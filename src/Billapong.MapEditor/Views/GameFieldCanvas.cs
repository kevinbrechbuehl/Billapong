namespace Billapong.MapEditor.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;
    using Models.Parameters;

    public class GameFieldCanvas : Canvas
    {
        #region Dependency properties

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(GameFieldCanvas), new PropertyMetadata(ClickCommandChanged));

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        #endregion

        public static void ClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (GameFieldCanvas)sender;
            if (canvas.DataContext is GameWindow)
            {
                var args = new GameWindowClickedArgs
                {
                    Point = e.GetPosition((IInputElement) sender),
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
