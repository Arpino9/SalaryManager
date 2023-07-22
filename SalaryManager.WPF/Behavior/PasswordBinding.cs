namespace SalaryManager.WPF.Behavior
{
    using System.Windows;
    using System.Windows.Controls;

    public class PasswordBinding : DependencyObject
    {
        public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.RegisterAttached(
            "IsAttached",
            typeof(bool),
            typeof(PasswordBinding),
            new FrameworkPropertyMetadata(false, PasswordBinding.IsAttachedProperty_Changed));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBinding),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PasswordBinding.PasswordProperty_Changed));

        public static bool GetIsAttached(DependencyObject dp)
        {
            return (bool)dp.GetValue(PasswordBinding.IsAttachedProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordBinding.PasswordProperty);
        }

        public static void SetIsAttached(DependencyObject dp, bool value)
        {
            dp.SetValue(PasswordBinding.IsAttachedProperty, value);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordBinding.PasswordProperty, value);
        }

        private static void IsAttachedProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordBinding.PasswordBox_PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordBinding.PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            PasswordBinding.SetPassword(passwordBox, passwordBox.Password);
        }

        private static void PasswordProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var newPassword = (string)e.NewValue;

            if (!GetIsAttached(passwordBox))
            {
                SetIsAttached(passwordBox, true);
            }

            if ((string.IsNullOrEmpty(passwordBox.Password) && string.IsNullOrEmpty(newPassword)) ||
                passwordBox.Password == newPassword)
            {
                return;
            }

            passwordBox.PasswordChanged -= PasswordBinding.PasswordBox_PasswordChanged;
            passwordBox.Password = newPassword;
            passwordBox.PasswordChanged += PasswordBinding.PasswordBox_PasswordChanged;
        }
    }
}
