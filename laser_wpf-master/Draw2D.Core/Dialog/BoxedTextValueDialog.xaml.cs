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

namespace Infini.Draw2D.Core.Dialog
{
    /// <summary>
    /// Interaction logic for BarcodeValueDialog.xaml
    /// </summary>
    public partial class BarcodeValueDialog : Window 
    {
        public BarcodeValueDialog(string defaultText = "")
        {
            InitializeComponent();
            txtInput.Text = defaultText;
            txtInput.Focus();
        }

        public string UserInput { get; private set; }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            UserInput = txtInput.Text;
            DialogResult = true; // Close the dialog and return true
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close the dialog and return false
        }
    }
}
