namespace Billapong.MapEditor.Views
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Class for handing passwordbox's password property via MVVM.
    /// </summary>
    public static class PasswordBoxBinder
    {
        /// <summary>
        /// The password field dependenc property
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxBinder),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="obj">The dependency object</param>
        /// <returns>The password from the dependency property.</returns>
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="value">The password value.</param>
        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Called when the password has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            passwordBox.PasswordChanged -= PasswordChanged;
            passwordBox.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// Password changed event, set the new password to the dependency property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            SetPassword(passwordBox, passwordBox.Password);
        }
    }
}
