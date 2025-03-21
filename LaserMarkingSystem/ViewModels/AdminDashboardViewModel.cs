using Caliburn.Micro;
using InIWorkspace.Commands;
using InIWorkspace.Data.Entities;
using InIWorkspace.Events;
using InIWorkspace.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    /// <summary>
    /// Represents a single data item for the Admin Dashboard, holding activation code details.
    /// </summary>
    public class DataSourceItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MacID { get; set; }
        public string ActivationCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// ViewModel for the Admin Dashboard, managing data and commands for the UI.
    /// Implements IHandle<NewKeyMessage> to respond to new key events.
    /// </summary>
    public class AdminDashboardViewModel : Screen, IHandle<NewKeyMessage>
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly SQLiteHelper _sqliteHelper;
        private DataSourceItem _selectedItem;

        /// <summary>
        /// Command to generate a new activation key.
        /// </summary>
        public ICommand GenerateKeyCommand { get; }

        /// <summary>
        /// Command to edit an existing activation key.
        /// </summary>
        public ICommand EditKeyCommand { get; }

        /// <summary>
        /// Command to delete an existing activation key.
        /// </summary>
        public ICommand DeleteKeyCommand { get; }

        /// <summary>
        /// Collection of data items displayed in the DataGrid.
        /// </summary>
        public ObservableCollection<DataSourceItem> DataSourceItems { get; set; }

        /// <summary>
        /// Gets or sets the currently selected data item.
        /// Notifies the UI of changes to support binding updates.
        /// </summary>
        public DataSourceItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
                // Notify CanExecute changes for commands that depend on SelectedItem
                (DeleteKeyCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditKeyCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the AdminDashboardViewModel.
        /// </summary>
        /// <param name="windowManager">Service for managing dialog windows.</param>
        /// <param name="eventAggregator">Service for publishing and subscribing to events.</param>
        public AdminDashboardViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _sqliteHelper = new SQLiteHelper("app.db");

            // Subscribe to events on the UI thread
            _eventAggregator.SubscribeOnUIThread(this);

            // Initialize the data collection with existing users
            DataSourceItems = new ObservableCollection<DataSourceItem>(GetUsers());

            // Initialize commands with their respective execute and can-execute logic
            GenerateKeyCommand = new RelayCommand(GenerateKey, CanGenerateKey);
            DeleteKeyCommand = new RelayCommand(DeleteKey, CanDelete);
            EditKeyCommand = new RelayCommand(EditKey, CanEdit);
        }

        /// <summary>
        /// Determines if a new key can be generated.
        /// </summary>
        /// <returns>Always true, allowing key generation at any time.</returns>
        private bool CanGenerateKey() => true;

        /// <summary>
        /// Determines if the selected key can be deleted.
        /// </summary>
        /// <returns>True if an item is selected, false otherwise.</returns>
        private bool CanDelete() => SelectedItem != null;

        /// <summary>
        /// Determines if the selected key can be edited.
        /// </summary>
        /// <returns>True if an item is selected, false otherwise.</returns>
        private bool CanEdit() => SelectedItem != null;

        /// <summary>
        /// Opens a dialog to generate a new activation key.
        /// </summary>
        private async void GenerateKey()
        {
            await _windowManager.ShowDialogAsync(new GenerateKeyViewModel(_eventAggregator));
        }

        /// <summary>
        /// Deletes the selected activation key from the database and UI collection.
        /// </summary>
        private void DeleteKey()
        {
            if (SelectedItem != null && DataSourceItems.Contains(SelectedItem))
            {
                _sqliteHelper.DeleteUser(SelectedItem.Id);
                DataSourceItems.Remove(SelectedItem);
                SelectedItem = null; // Clear selection after deletion
            }
        }

        /// <summary>
        /// Opens a dialog to edit the selected activation key.
        /// </summary>
        private async void EditKey()
        {
            if (SelectedItem != null && DataSourceItems.Contains(SelectedItem))
            {
                await _windowManager.ShowDialogAsync(new GenerateKeyViewModel(
                    _eventAggregator,
                    SelectedItem.CustomerName,
                    SelectedItem.ActivationCode,
                    SelectedItem.MacID,
                    true,
                    SelectedItem.Id));
                SelectedItem.Status = "Updated"; // Update status after editing
            }
        }

        /// <summary>
        /// Handles the NewKeyMessage event to add or update a data item in the collection.
        /// </summary>
        /// <param name="message">The message containing new or updated key data.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A completed task.</returns>
        public Task HandleAsync(NewKeyMessage message, CancellationToken cancellationToken)
        {
            if (message.IsUpdate)
            {
                // Update an existing user in the database
                _sqliteHelper.UpdateUser(new User
                {
                    Id = message.Id,
                    Username = message.Name,
                    ActivationCode = message.Key.Split(' '),
                    MacAddress = message.MacAddress,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    IsActive = false
                });

                // Replace the old item with the updated one in the collection
                var item = DataSourceItems.First(p => p.Id == message.Id);
                DataSourceItems.Remove(item);
                DataSourceItems.Add(new DataSourceItem
                {
                    Id = message.Id,
                    CustomerName = message.Name,
                    MacID = message.MacAddress,
                    ActivationCode = message.Key,
                    CreatedDate = DateTime.Now,
                    Status = "Not Active" // Standardized casing
                });
            }
            else
            {
                // Create and insert a new user into the database
                var newUser = new User
                {
                    Username = message.Name,
                    ActivationCode = message.Key.Split(' '),
                    MacAddress = message.MacAddress,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    IsActive = false
                };
                _sqliteHelper.InsertUser(newUser);

                // Add the new item to the collection
                DataSourceItems.Add(new DataSourceItem
                {
                    Id = newUser.Id,
                    CustomerName = newUser.Username,
                    MacID = newUser.MacAddress,
                    ActivationCode = message.Key,
                    CreatedDate = DateTime.Now,
                    Status = "Not Active" // Standardized casing
                });
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves all users from the database and maps them to DataSourceItem objects.
        /// </summary>
        /// <returns>A list of DataSourceItem objects representing the users.</returns>
        private System.Collections.Generic.List<DataSourceItem> GetUsers() =>
            _sqliteHelper.GetAllUsers()
                .Select(user => new DataSourceItem
                {
                    Id = user.Id,
                    CustomerName = user.Username,
                    MacID = user.MacAddress,
                    ActivationCode = string.Join(" ", user.ActivationCode),
                    CreatedDate = DateTime.Parse(user.CreatedDate),
                    Status = user.IsActive ? "Active" : "Not Active"
                })
                .ToList();
    }
}