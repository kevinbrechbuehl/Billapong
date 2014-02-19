namespace Billapong.GameConsole.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Models;

    public class PlayerRoundStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Player.RoundState)value == Player.RoundState.BallMoving || (Player.RoundState)value == Player.RoundState.BallPlaced)
            {
                return Visibility.Visible;
            }
                
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
