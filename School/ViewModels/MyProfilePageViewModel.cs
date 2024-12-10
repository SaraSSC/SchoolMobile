using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Models;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace School.ViewModels
{
	public class MyProfilePageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;
        private StudentResponse _student;
        private string _picturePath;
        private string _profilePicturePath;
        private string _birthDate;
        private bool _isRunning;

        public MyProfilePageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            LoadStudentAsync();
        }

        public StudentResponse Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }

        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set => SetProperty(ref _profilePicturePath, value);
        }

        public string PicturePath
        {
            get => _picturePath;
            set => SetProperty(ref _picturePath, value);
        }

        public string BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private async void LoadStudentAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Oops", "No Internet connection", "Accept");
                });

                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            if (!mainViewModel.IsTokenValid() || !mainViewModel.IsEmailSaved())
            {
                await App.Current.MainPage.DisplayAlert("Oops", "Insuficient user information", "Accept");
                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["ApiBaseUrl"].ToString();
            string prefix = App.Current.Resources["ApiSchoolPrefix"].ToString();
            string controller = App.Current.Resources["ApiSchoolStudent"].ToString() + $"/{mainViewModel.Email}";

            Response response = await _apiService.GetSingleResultAsync<StudentResponse>
                (url, prefix, controller, "bearer", MainViewModel.GetInstance().Token.Token);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Oops", response.Message, "Accept");
                return;
            }

            Student = response.Result as StudentResponse;

            ProfilePicturePath = App.Current.Resources["ApiBaseUrl"].ToString() + Student.ProfilePicturePath.Replace("~", "");
            PicturePath = App.Current.Resources["ApiBaseUrl"].ToString() + Student.PicturePath.Replace("~", "");
            BirthDate = Student.BirthDate.ToString().Split(' ')[0];
        }
    }
}

