using Caliburn.Micro;
using InIWorkspace.Commands;
using InIWorkspace.Events;
using InIWorkspace.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    public class GenerateKeyViewModel : Screen
    {
        // Fields for state and dependencies
        private readonly IEventAggregator _eventAggregator;
        private int _id;
        private string _name;
        private string _macAddress;
        private string _key;
        private bool _isUpdate;

        // Properties with change notification
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string MacAddress
        {
            get => _macAddress;
            set
            {
                _macAddress = value;
                NotifyOfPropertyChange(() => MacAddress);
            }
        }

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyOfPropertyChange(() => Key);
            }
        }

        public bool IsUpdate
        {
            get => _isUpdate;
            set
            {
                _isUpdate = value;
                NotifyOfPropertyChange(() => IsUpdate);
            }
        }

        // Commands for UI interaction
        public ICommand FetchMacAddressCommand { get; }
        public ICommand GenerateKeyCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddKeyCommand { get; }

        // Default constructor for new key generation
        public GenerateKeyViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            FetchMacAddressCommand = new RelayCommand(FetchMacAddress);
            GenerateKeyCommand = new RelayCommand(GenerateKey, CanGenerateKey);
            CancelCommand = new RelayCommand(Cancel);
            AddKeyCommand = new RelayCommand(AddKey);

            // Initialize properties to ensure placeholders are visible
            Name = string.Empty;
            MacAddress = string.Empty;
            Key = string.Empty;
        }

        // Parameterized constructor for editing existing key
        public GenerateKeyViewModel(IEventAggregator eventAggregator, string name, string key, string macAddress, bool isUpdate, int id)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            FetchMacAddressCommand = new RelayCommand(FetchMacAddress);
            GenerateKeyCommand = new RelayCommand(GenerateKey, CanGenerateKey);
            CancelCommand = new RelayCommand(Cancel);
            AddKeyCommand = new RelayCommand(AddKey);

            Id = id;
            Name = name;
            Key = key;
            MacAddress = macAddress;
            IsUpdate = isUpdate;
        }

        // Fetches and formats the system's MAC address
        private void FetchMacAddress()
        {
            var tempMacAddress = MacAddressHelper.GetMacAddress();
            MacAddress = MacAddressHelper.FormatMacAddress(tempMacAddress);
        }

        // Determines if key generation is allowed
        private bool CanGenerateKey()
        {
            return !string.IsNullOrEmpty(MacAddress);
        }

        // Generates a random key and assigns it
        private void GenerateKey()
        {
            Key = GenerateRandomKey(); // Using random key generation by default
        }

        // Closes the view
        private void Cancel()
        {
            TryCloseAsync();
        }

        // Publishes the key and closes the view
        private async void AddKey()
        {
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Customer Name cannot be empty", "Customer Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(MacAddress))
            {
                MessageBox.Show("Mac ID cannot be empty", "Mac Address", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Key))
            {
                MessageBox.Show("Activation Code cannot be empty", "Activate Code", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var message = new NewKeyMessage
            {
                Id = Id,
                Key = Key,
                MacAddress = MacAddress,
                Name = Name,
                IsUpdate = IsUpdate
            };

            await _eventAggregator.PublishOnUIThreadAsync(message);
            await TryCloseAsync();
        }

        // Generates a 4-part random activation code (16 digits, spaced every 4)
        private static string GenerateRandomKey()
        {
            Random random = new Random();
            string key = string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
            return string.Join(" ", Enumerable.Range(0, 4).Select(i => key.Substring(i * 4, 4)));
        }

         
    }
}