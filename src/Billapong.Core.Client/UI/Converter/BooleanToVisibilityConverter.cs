namespace Billapong.Core.Client.UI.Converter
{
    using System.Windows;

    /// <summary>
    /// Boolean to visibility converter.
    /// </summary>
    public class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToVisibilityConverter"/> class.
        /// </summary>
        public BooleanToVisibilityConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
