using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace School.ViewModels
{
    public class AboutPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;
        public AboutPageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Logo = "ic_icon_logabout.png";
            Information = "Application made by:\n" + "SaraSSC\n" + "Version 2.0";
        }
        public string Logo { get; set; }
        public string Information { get; set; }
    }
}
