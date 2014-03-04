namespace Billapong.GameConsole.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Models;

    /// <summary>
    /// Converts the given player state into the visibility of the control
    /// </summary>
    public class PlayerStateToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playerState = (Player.PlayerState)value;

            if (playerState == Player.PlayerState.OpponentsTurn || playerState == Player.PlayerState.Lost)
            {
                return Visibility.Collapsed;
            }
                
            return Visibility.Visible;
        }

        /// <summary>
        /// Converts a value. The method is not fully implemented and is not needed
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Player.PlayerState.OpponentsTurn;
        }
    }
}
