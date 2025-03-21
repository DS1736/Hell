using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace InIWorkspace.Views
{
    /// <summary>
    /// Interaction logic for UserLoginView.xaml
    /// </summary>
    public partial class UserLoginView : UserControl
    {
        private void txtFirst_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits (0-9)
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        private void txtSecond_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits (0-9)
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        private void txtThird_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits (0-9)
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        private void txtFour_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits (0-9)
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        public UserLoginView()
        {
            InitializeComponent();
        }
    }
}
