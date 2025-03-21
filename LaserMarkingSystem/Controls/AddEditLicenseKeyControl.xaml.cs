using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InIWorkspace.Controls
{
    /// <summary>
    /// Interaction logic for AddEditLicenseKeyControl.xaml
    /// </summary>
    public partial class AddEditLicenseKeyControl : UserControl
    {
        public AddEditLicenseKeyControl()
        {
            InitializeComponent();
        }
        private void FetchMacID_Click(object sender, RoutedEventArgs e)
        {
            // Logic to fetch Mac ID (Replace with actual logic)
            MacIDTextBox.Text = "00-14-22-01-23-45";
        }

        private void GenerateActivationCode_Click(object sender, RoutedEventArgs e)
        {
            // Logic to generate a 16-digit activation code
            ActivationCodeTextBox.Text = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close or hide the control (assuming it's within a modal popup)
            this.Visibility = Visibility.Collapsed;
        }

        private void AddKey_Click(object sender, RoutedEventArgs e)
        {
            string customerName = CustomerNameTextBox.Text;
            string macID = MacIDTextBox.Text;
            string activationCode = ActivationCodeTextBox.Text;

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(macID) || string.IsNullOrWhiteSpace(activationCode))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Logic to save activation key (Placeholder)
            MessageBox.Show("Activation key added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close or reset the control after adding the key
            this.Visibility = Visibility.Collapsed;
        }
    }
}
