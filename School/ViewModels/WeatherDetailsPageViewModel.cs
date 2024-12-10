using Example;
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
	public class WeatherDetailsPageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;
        private WeatherResponse _weather;
        private bool _isRunning;
        private string _iconPath;
        private string _currentTime;
        public WeatherDetailsPageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            Title = "Weather Details";
            _apiService = apiService;

            ColorBrown = App.Current.Resources["ColorBrown"].ToString();
        }

        public string Search { get; set; }

        public string ColorBrown { get; set; }

        public WeatherResponse Weather
        {
            get => _weather;
            set => SetProperty(ref _weather, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        public string CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("weather"))
            {
                Search = parameters.GetValue<string>("weather");
                Title = Search;
            }

            LoadWeatherDetails();
        }

        private async void LoadWeatherDetails()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Oops", "No Internet connection", "Accept");
                });

                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["ApiWeatherBaseUrl"].ToString();
            string prefix = App.Current.Resources["ApiWeatherPrefix"].ToString();
            string controller = $"{App.Current.Resources["ApiWeatherCity"]}?q={Search}&appid={App.Current.Resources["ApiWeatherKey"]}&units=metric";

            Response response = await _apiService.GetSingleResultAsync<WeatherResponse>(url, prefix, controller);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Oops", $"'{Search}' not found", "Accept");
                return;
            }

            Weather = response.Result as WeatherResponse;

            UpdateInformation();
        }

        private void UpdateInformation()
        {
            IconPath = $"{App.Current.Resources["UrlIconWeather"]}/{Weather.Weather[0].Icon}.png";

            TimeSpan timeSpan = TimeSpan.FromSeconds(Weather.Dt);
            CurrentTime = (new DateTime() + timeSpan).ToString("H:mm");


        }
    }
}
