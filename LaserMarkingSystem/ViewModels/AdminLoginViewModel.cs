using Caliburn.Micro;
using InIWorkspace.Commands;
using InIWorkspace.Events;
using InIWorkspace.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    public class AdminLoginViewModel : Screen
    {
        // Fields for UI state and dependencies
        private string _username;
        private string _password;
        private string _usernameError;
        private string _passwordError;
        private string _visiblePassword;
        private Visibility _passwordBoxVisibility = Visibility.Visible;
        private Visibility _textBoxVisibility = Visibility.Collapsed;
        private PackIconKind _eyeIconKind = PackIconKind.EyeOutline;
        private readonly IEventAggregator _eventAggregator;
        private readonly SQLiteHelper _sqliteHelper;

        // Commands for UI interaction
        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        // Username property with change notification
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        // Password property with change notification
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        // Visible password property, syncs with Password when visible
        public string VisiblePassword
        {
            get => _visiblePassword;
            set
            {
                _visiblePassword = value;
                NotifyOfPropertyChange(() => VisiblePassword);
                if (TextBoxVisibility == Visibility.Visible)
                {
                    Password = value; // Sync Password when TextBox is visible
                }
            }
        }

        // Visibility of PasswordBox in UI
        public Visibility PasswordBoxVisibility
        {
            get => _passwordBoxVisibility;
            set
            {
                _passwordBoxVisibility = value;
                NotifyOfPropertyChange(() => PasswordBoxVisibility);
            }
        }

        // Visibility of TextBox for visible password
        public Visibility TextBoxVisibility
        {
            get => _textBoxVisibility;
            set
            {
                _textBoxVisibility = value;
                NotifyOfPropertyChange(() => TextBoxVisibility);
            }
        }

        // Eye icon state for password visibility toggle
        public PackIconKind EyeIconKind
        {
            get => _eyeIconKind;
            set
            {
                _eyeIconKind = value;
                NotifyOfPropertyChange(() => EyeIconKind);
            }
        }

        // Error message for username input
        public string UsernameError
        {
            get => _usernameError;
            set
            {
                _usernameError = value;
                NotifyOfPropertyChange(() => UsernameError);
            }
        }

        // Error message for password input
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                NotifyOfPropertyChange(() => PasswordError);
            }
        }

        // Constructor with dependency injection
        public AdminLoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _sqliteHelper = new SQLiteHelper("app.db");

            LoginCommand = new RelayCommand(Login);
            TogglePasswordVisibilityCommand = new RelayCommand(ExecuteTogglePasswordVisibility); // Line 129
            // Default credentials for testing
            Username = "janit@newnop.net";
            Password = "Welcome123#";
        }

        // Handles login button click
        private void Login()
        {
            // Clear previous error messages
            UsernameError = string.Empty;
            PasswordError = string.Empty;

            bool hasError = false;

            // Validate username
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Username is required";
                hasError = true;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                hasError = true;
            }

            if (hasError)
            {
                return; // Exit if validation fails
            }

            // Check credentials against database
            if (_sqliteHelper.ValidateUser(Username, Password))
            {
                _eventAggregator.PublishOnUIThreadAsync(new AdminLoginSuccessMessage());
                TryCloseAsync(); // Close the view on success
            }
            else
            {
                // Display error for invalid credentials
                UsernameError = "Username is required";
                PasswordError = "Password is required";
            }
        }

        // Toggles password visibility between PasswordBox and TextBox
        private void ExecuteTogglePasswordVisibility()
        {
            if (PasswordBoxVisibility == Visibility.Visible)
            {
                VisiblePassword = Password ?? string.Empty; // Ensure null password becomes empty string
                PasswordBoxVisibility = Visibility.Collapsed;
                TextBoxVisibility = Visibility.Visible;
                EyeIconKind = PackIconKind.EyeOffOutline;
            }
            else
            {
                Password = VisiblePassword ?? string.Empty; // Ensure null VisiblePassword becomes empty string
                PasswordBoxVisibility = Visibility.Visible;
                TextBoxVisibility = Visibility.Collapsed;
                EyeIconKind = PackIconKind.EyeOutline;
            }
        }
    }
}