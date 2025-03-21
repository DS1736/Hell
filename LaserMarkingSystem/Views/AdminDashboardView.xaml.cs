using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using InIWorkspace.ViewModels;

namespace InIWorkspace.Views
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for the three-dot button in each DataGrid row.
        /// Displays a context menu with Edit and Delete options with icons.
        /// </summary>
        /// <param name="sender">The button that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;

            if (button.DataContext is not DataSourceItem data) return;

            // Set the selected item in the ViewModel
            if (DataContext is AdminDashboardViewModel viewModel)
            {
                viewModel.SelectedItem = data;
            }

            var menu = new ContextMenu
            {
                Style = (Style)FindResource("CustomContextMenuStyle")
            };

            var editMenuItem = new MenuItem
            {
                Header = "Edit",
                Style = (Style)FindResource("CustomMenuItemStyle"),
                Icon = CreateIcon("pack://application:,,,/Resources/Images/EditIcon.png")
            };
            editMenuItem.Click += (s, args) => EditItem();
            menu.Items.Add(editMenuItem);

            var deleteMenuItem = new MenuItem
            {
                Header = "Delete",
                Style = (Style)FindResource("CustomMenuItemStyle"),
                Icon = CreateIcon("pack://application:,,,/Resources/Images/DeleteIcon.png")
            };
            deleteMenuItem.Click += (s, args) => DeleteItem();
            menu.Items.Add(deleteMenuItem);

            button.ContextMenu = menu;
            menu.IsOpen = true;
        }

        private Image CreateIcon(string iconPath)
        {
            try
            {
                return new Image
                {
                    Source = new BitmapImage(new Uri(iconPath, UriKind.Absolute)),
                    Width = 16,
                    Height = 16
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load icon at {iconPath}: {ex.Message}");
                return null;
            }
        }

        private void EditItem()
        {
            if (DataContext is AdminDashboardViewModel viewModel && viewModel.EditKeyCommand.CanExecute(null))
            {
                viewModel.EditKeyCommand.Execute(null);
            }
        }

        private void DeleteItem()
        {
            if (DataContext is AdminDashboardViewModel viewModel && viewModel.DeleteKeyCommand.CanExecute(null))
            {
                viewModel.DeleteKeyCommand.Execute(null);
            }
        }
    }
}