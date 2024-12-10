using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Models;
using School.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace School.ViewModels
{
    public class MyEvaluationDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;

        private CourseResponse _course;
        private ObservableCollection<EvaluationResponse> _evaluations;
        private bool _isRunning;
        private string _noEval;
        public MyEvaluationDetailsPageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            Title = "My Evaluations";
        }

        public CourseResponse Course
        {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        public ObservableCollection<EvaluationResponse> Evaluations
        {
            get => _evaluations;
            set => SetProperty(ref _evaluations, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string NoEval
        {
            get => _noEval;
            set => SetProperty(ref _noEval, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("course"))
            {
                Course = parameters.GetValue<CourseResponse>("course");
                LoadEvaluationsAsync();
            }
        }

        private async void LoadEvaluationsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Something went wrong:", "No Internet connection available", "Ok");
                });


                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            if (!mainViewModel.IsTokenValid() || !mainViewModel.IsEmailSaved())
            {
                await App.Current.MainPage.DisplayAlert("Data Missing:", "Please verify your profile information on the web service, some data is missing", "Ok");

                return;
            }




            IsRunning = true;

            string url = App.Current.Resources["ApiBaseUrl"].ToString();
            string prefix = App.Current.Resources["ApiSchoolPrefix"].ToString();
            string controller = App.Current.Resources["ApiSchoolStudentEvaluations"].ToString() + $"/{mainViewModel.Email}" + $"/{Course.Id}";

            Response response = await _apiService.GetMultipleResultsAsync<EvaluationResponse>
                (url, prefix, controller, "bearer", MainViewModel.GetInstance().Token.Token);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Ok");
                return;
            }

            Evaluations = new ObservableCollection<EvaluationResponse>(response.Result as List<EvaluationResponse>);

            if (Evaluations is null || Evaluations.Count <= 0)
            {
                NoEval = "No evaluations found";
            }
        }
    }
}
