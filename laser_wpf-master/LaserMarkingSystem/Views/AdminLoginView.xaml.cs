using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf; // For PackIconKind

namespace InIWorkspace.Views
{
    public partial class AdminLoginView : UserControl
    {
        private bool _isPasswordVisible = false;

        public AdminLoginView()
        {
            InitializeComponent();
            txtPassword.Password = "Welcome123#"; // Default password for testing
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                // Hide password
                txtPassword.Password = txtPasswordVisible.Text;
                passwordBoxBorder.Visibility = Visibility.Visible;
                textBoxBorder.Visibility = Visibility.Collapsed;
                eyeIcon.Kind = PackIconKind.EyeOutline; // Show "eye" icon
                _isPasswordVisible = false;
            }
            else
            {
                // Show password
                txtPasswordVisible.Text = txtPassword.Password;
                passwordBoxBorder.Visibility = Visibility.Collapsed;
                textBoxBorder.Visibility = Visibility.Visible;
                eyeIcon.Kind = PackIconKind.EyeOffOutline; // Show "eye off" icon
                _isPasswordVisible = true;
            }
        }
    }
}