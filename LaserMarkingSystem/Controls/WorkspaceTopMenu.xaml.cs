using Microsoft.Win32;
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
    /// Interaction logic for WorkspaceTopMenu.xaml
    /// </summary>
    public partial class WorkspaceTopMenu : UserControl
    {
        public WorkspaceTopMenu()
        {
            InitializeComponent();
        }

        // Declare an event to notify the main window
        public event Action<string> FileSelected;

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();            

            if (openFileDialog.ShowDialog() == true)
            {
                // Raise the FileSelected event with the selected file name
                FileSelected?.Invoke(openFileDialog.FileName);
            }
        }
    }
}
