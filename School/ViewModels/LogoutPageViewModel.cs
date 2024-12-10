using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace School.ViewModels
{
	public class LogoutPageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
        private DelegateCommand _loadedCommand;

        private bool _isRunning = false;
        private bool _isActive = true;
        public LogoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Logout();
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public DelegateCommand LoadedCommand => _loadedCommand
            ?? (_loadedCommand = new DelegateCommand(OnLoadedRedirectAsync));

        private async void OnLoadedRedirectAsync()
        {


            IsActive = true;
            IsRunning = true;

            await _navigationService.NavigateAsync($"NavigationPage/{nameof(LoginPage)}");
            IsRunning = false;
        }


        private void Logout()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = null;
            mainViewModel.Email = string.Empty;
        }
    }
}
