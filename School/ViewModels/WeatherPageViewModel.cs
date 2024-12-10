using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Models;
using School.Services;
using School.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace School.ViewModels
{
    public class WeatherPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;
        private DelegateCommand _searchCommand;
        private DelegateCommand _countryCommand;
        private DelegateCommand _cityCommand;
        private WeatherResponse _weather;
        private ObservableCollection<string> _countries;
        private string _selectedCountry;
        private ObservableCollection<string> _cities;
        private string _selectedCity;
        private bool _isRunning;
        private string _search;

        public WeatherPageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Title = "Weather";
            GetCountriesAsync();
        }

        public DelegateCommand SearchCommand =>
           _searchCommand ?? (_searchCommand = new DelegateCommand(GoToWeatherDetailsFromSearchAsync));

        public DelegateCommand CountryCommand =>
            _countryCommand ?? (_countryCommand = new DelegateCommand(GetCountryCitiesAsync));

        public DelegateCommand CityCommand =>
            _cityCommand ?? (_cityCommand = new DelegateCommand(GoToWeatherDetailsFromSelectAsync));

        public WeatherResponse Weather
        {
            get => _weather;
            set => SetProperty(ref _weather, value);
        }

        public ObservableCollection<string> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                SetProperty(ref _selectedCountry, value);
                GetCountryCitiesAsync();
            }
        }

        public ObservableCollection<string> Cities
        {
            get => _cities;
            set => SetProperty(ref _cities, value);
        }

        public string SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);

        }

        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string ColorBrown { get; set; }

        private async void GoToWeatherDetailsFromSearchAsync()
        {
            if (string.IsNullOrEmpty(Search))
            {
                await App.Current.MainPage.DisplayAlert("Don't you forgot anything?", "Gotta search for something", "Accept");
                return;
            }

            if (Search.Length < 2)
            {
                await App.Current.MainPage.DisplayAlert("Note:", "Two characters minimum for the search to work", "Accept");
                return;
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "weather", Search }
            };

            await _navigationService.NavigateAsync(nameof(WeatherDetailsPage), parameters);
        }

        private async void GetCountriesAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Something went wrong:", "No Internet connection", "Accept");
                });

                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["ApiCountriesBaseUrl"].ToString();
            string prefix = App.Current.Resources["ApiCountriesPrefix"].ToString();
            string controller = App.Current.Resources["ApiCountriesAll"].ToString();

            Response response = await _apiService.GetSingleResultAsync<CountryResponse>(url, prefix, controller);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Oops", $"'{Search}' not found", "Accept");
                return;
            }

            var countriesRaw = response.Result as CountryResponse;
            var countriesTemp = new ObservableCollection<string>();

            if (countriesRaw.Data.Count() > 0)
            {
                foreach (var item in countriesRaw.Data)
                {
                    countriesTemp.Add(item.Name);
                }
            }

            Countries = countriesTemp;
        }

        private async void GetCountryCitiesAsync()
        {
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                return;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Something went wrong:", "No Internet connection", "Accept");
                });

                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["ApiCitiesBaseUrl"].ToString();
            string prefix = App.Current.Resources["ApiCitiesPrefix"].ToString();
            string controller = App.Current.Resources["ApiCitiesAll"].ToString();

            Response response = await _apiService.PostCitiesAsync<CityResponse>
                (url, prefix, controller, new CityRequest { Country = SelectedCountry });

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Oops", $"'{SelectedCountry}' not found", "Accept");
                return;
            }

            var citiesRaw = response.Result as CityResponse;
            var citiesTemp = new ObservableCollection<string>();

            if (citiesRaw.Data.Count() > 0)
            {
                foreach (var item in citiesRaw.Data)
                {
                    citiesTemp.Add(item);
                }
            }

            Cities = citiesTemp;
        }

        private async void GoToWeatherDetailsFromSelectAsync()
        {
            if (string.IsNullOrEmpty(SelectedCity))
            {
                await App.Current.MainPage.DisplayAlert("Don't you forgot anything?", "Gotta search for something", "Accept");
                return;
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "weather", SelectedCity }
            };

            await _navigationService.NavigateAsync(nameof(WeatherDetailsPage), parameters);
        }
    }
}
