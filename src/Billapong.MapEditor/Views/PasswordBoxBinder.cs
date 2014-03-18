using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billapong.MapEditor.Views
{
    using System.Windows;
    using System.Windows.Controls;

    public static class PasswordBoxBinder
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxBinder),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            passwordBox.PasswordChanged -= PasswordChanged;
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            SetPassword(passwordBox, passwordBox.Password);
        }
    }
}
