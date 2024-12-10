using Example;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Models;
using School.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using School.Services;

namespace School.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ApiService _apiService;
        private DelegateCommand _loginCommand;
        private string _password;
        private bool _isRunning = false;
        private bool _isEnabled = true;

        public LoginPageViewModel(INavigationService navigationService)
           : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = new ApiService();
            IsEnabled = true;

        }
       
        public DelegateCommand LoginCommand =>
           _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Something went wrong:", "No Internet connection available", "Ok");
                });

                Password = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Missing:", "Please enter your email", "Ok");
                Password = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Missing:", "Please enter your password", "Ok");
                Password = string.Empty;
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new TokenRequest
            {
                Username = this.Email,
                Password = this.Password
            };

            string url = App.Current.Resources["ApiBaseUrl"].ToString();
            string controller = App.Current.Resources["ApiCreateToken"].ToString();

            var response = await _apiService.GetTokenAsync(url, controller, request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Something went wrong:", "Email or Password incorrenct. Please try again", "Ok");
                Password = string.Empty;
                return;
            }

            var token = response.Result as TokenResponse;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Email = Email;

            if (mainViewModel.Token == null)
            {
                Debug.WriteLine("Token is null, navigating to LoginPage");
                await _navigationService.NavigateAsync
                    ($"/{nameof(SchoolMasterDetailsPage)}/NavigationPage/{nameof(LoginPage)}");
            }

            if (mainViewModel.IsTokenValid() && mainViewModel.IsEmailSaved())
            {
                Debug.WriteLine("Token is valid and email is saved, navigating to MyProfilePage");
                await _navigationService.NavigateAsync
                    ($"/{nameof(SchoolMasterDetailsPage)}/NavigationPage/{nameof(MyProfilePage)}");

            }
            else
            {

                Debug.WriteLine($"Navigation to MyProfilePage failed");

            }
        }
    }
    
}
