namespace Billapong.GameConsole.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Models;

    public class PlayerStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playerState = (Player.PlayerState) value;

            if (playerState == Player.PlayerState.OpponentsTurn || playerState == Player.PlayerState.Lost)
            {
                return Visibility.Collapsed;
            }
                
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
