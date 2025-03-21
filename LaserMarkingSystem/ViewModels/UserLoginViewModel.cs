using Caliburn.Micro;
using InIWorkspace.Commands;
using InIWorkspace.Events;
using InIWorkspace.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace InIWorkspace.ViewModels
{
    public class UserLoginViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        // ActivateCommand
        public ICommand ActivateCommand { get; }

        private string _codePart1 { get; set; }
        private string _codePart2 {  get; set; }
        private string _codePart3 { get; set; }
        private string _codePart4 { get; set; }


        private readonly SQLiteHelper _sqliteHelper;

        public string CodePart1
        {
            get => _codePart1;
            set
            {
                _codePart1 = value;
                NotifyOfPropertyChange(() => CodePart1);
            }
        }
        public string CodePart2
        {
            get => _codePart2;
            set
            {
                _codePart2 = value;
                NotifyOfPropertyChange(() => CodePart2);
            }
        }
        public string CodePart3
        {
            get => _codePart3;
            set
            {
                _codePart3 = value;
                NotifyOfPropertyChange(() => CodePart3);
            }
        }
        public string CodePart4
        {
            get => _codePart4;
            set
            {
                _codePart4 = value;
                NotifyOfPropertyChange(() => CodePart4);
            }
        }
     
        public UserLoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _sqliteHelper = new SQLiteHelper("app.db");

            ActivateCommand = new RelayCommand(Activate, null);
        }

        private void Activate()
        {
            if (_eventAggregator != null)
            {
                string activationCode = $"{CodePart1} {CodePart2} {CodePart3} {CodePart4}";

                if (string.IsNullOrEmpty(activationCode) || activationCode.Trim().Length<16)
                {
                    MessageBox.Show("Please enter a valid 16-digit activation code.", "Invalid Code", MessageBoxButton.OK, MessageBoxImage.Warning);
                } 
                else
                {
                    if (CodePart1.Length == 4 && CodePart2.Length == 4 &&
                CodePart3.Length == 4 && CodePart4.Length == 4)
                    {
                        if (_sqliteHelper.ValidateActivationCode(activationCode))
                        {
                            _sqliteHelper.UpdateLicenseStatus(activationCode);
                            _eventAggregator.PublishOnUIThreadAsync(new UserLoginMessage());
                        }
                        else
                        {
                            MessageBox.Show("Invalid activation code.", "Invalid Code", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid 16-digit activation code.", "Invalid Code", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            
               
            }
        }
    }
}
