namespace Billapong.GameConsole.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class GameFieldCanvas : Canvas
    {
        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof (ICommand), typeof (GameFieldCanvas), new PropertyMetadata(ClickCommandChanged));

        public ICommand ClickCommand
        {
            get { return (ICommand) GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public static void ClickCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var canvas = (GameFieldCanvas)obj;

            canvas.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            canvas.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (GameFieldCanvas)sender;
            if (canvas.ClickCommand.CanExecute(null))
            {
                canvas.ClickCommand.Execute(null);
            }
        }
    }
}
