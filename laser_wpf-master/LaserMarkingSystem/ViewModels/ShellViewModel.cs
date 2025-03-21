using Caliburn.Micro;
using InIWorkspace.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InIWorkspace.ViewModels
{
    public class ShellViewModel : Conductor<object>, IShell, IHandle<LandingSelectionMessage>, IHandle<AdminLogoutMessage>, IHandle<AdminLoginSuccessMessage>, IHandle<UserLoginMessage>
    {
        private readonly LandingViewModel _landingViewModel;
        private readonly AdminLoginViewModel _adminLoginViewModel;
        private readonly AdminDashboardViewModel _adminDashboardViewModel;
        private readonly UserLoginViewModel _userLoginViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly WorkspaceViewModel _workspaceViewModel;

        public ShellViewModel(LandingViewModel landingViewModel, AdminLoginViewModel adminLoginViewModel, AdminDashboardViewModel adminDashboardViewModel, UserLoginViewModel userLoginViewModel, WorkspaceViewModel workspaceViewModel, IEventAggregator eventAggregator)
        {
            _landingViewModel = landingViewModel;
            _adminLoginViewModel = adminLoginViewModel;
            _adminDashboardViewModel = adminDashboardViewModel;
            _userLoginViewModel = userLoginViewModel;
            _workspaceViewModel = workspaceViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this); // Subscribe to events


            // Load default screen
            ActivateItemAsync(_landingViewModel);
        }

        public Task HandleAsync(LandingSelectionMessage message, CancellationToken cancellationToken)
        {
            if (message.SelectedOption == LandingSelectionMessage.Option.Admin)
            {
                ActivateItemAsync(_adminLoginViewModel);
            }
            else if (message.SelectedOption == LandingSelectionMessage.Option.Operator)
            {
                ActivateItemAsync(_userLoginViewModel);
            }
            return Task.CompletedTask;
        }

        public Task HandleAsync(AdminLoginSuccessMessage message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_adminDashboardViewModel);
            return Task.CompletedTask;
        }

        public Task HandleAsync(AdminLogoutMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(UserLoginMessage message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_workspaceViewModel);
            return Task.CompletedTask;
        }
    }
}
