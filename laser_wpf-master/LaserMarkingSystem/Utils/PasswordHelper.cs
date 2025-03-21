using System.Windows;
using System.Windows.Controls;

namespace InIWorkspace.Utils
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPasswordBoxProperty =
            DependencyProperty.RegisterAttached("BindPasswordBox", typeof(bool), typeof(PasswordHelper),
                new PropertyMetadata(false, OnBindPasswordBoxChanged));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper),
                new PropertyMetadata(false));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        public static bool GetBindPasswordBox(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindPasswordBoxProperty);
        }

        public static void SetBindPasswordBox(DependencyObject obj, bool value)
        {
            obj.SetValue(BindPasswordBoxProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject obj, bool value)
        {
            obj.SetValue(IsUpdatingProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox && !GetIsUpdating(passwordBox))
            {
                // Prevent recursive updates
                SetIsUpdating(passwordBox, true);
                passwordBox.Password = (string)e.NewValue;
                SetIsUpdating(passwordBox, false);
            }
        }

        private static void OnBindPasswordBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
                else
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && !GetIsUpdating(passwordBox))
            {
                // Prevent recursive updates
                SetIsUpdating(passwordBox, true);
                SetBoundPassword(passwordBox, passwordBox.Password);
                SetIsUpdating(passwordBox, false);
            }
        }
    }
}