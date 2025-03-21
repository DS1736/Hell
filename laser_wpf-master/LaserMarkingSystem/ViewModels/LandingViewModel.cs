using Caliburn.Micro;
using InIWorkspace.Commands;
using InIWorkspace.Events;
using InIWorkspace.Utils;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InIWorkspace.ViewModels
{
    public class LandingViewModel : Screen
    {
        private IEventAggregator _eventAggregator;
        public ICommand AdminLoginSelectedCommand { get; }
        public ICommand UserLoginSelectedCommand { get; }
        private readonly SQLiteHelper _sqliteHelper;

        public LandingViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _sqliteHelper = new SQLiteHelper("app.db");

            AdminLoginSelectedCommand = new RelayCommand(AdminLogin, null);
            UserLoginSelectedCommand = new RelayCommand(UserLogin, null);
        }

        private void AdminLogin()
        {
            if (_eventAggregator != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new LandingSelectionMessage() { SelectedOption = LandingSelectionMessage.Option.Admin });
            }
        }

        private void UserLogin()
        {

            if (_eventAggregator != null)
            {
                var tempMacAddress =MacAddressHelper.GetMacAddress();
                var MacAddress = MacAddressHelper.FormatMacAddress(tempMacAddress);
                if (_sqliteHelper.GetLicenseStatus(MacAddress))
                {
                    _eventAggregator.PublishOnUIThreadAsync(new UserLoginMessage());
                }
                else
                {
                    _eventAggregator.PublishOnUIThreadAsync(new LandingSelectionMessage() { SelectedOption = LandingSelectionMessage.Option.Operator });
                }

            }
        }
        
    }
}
