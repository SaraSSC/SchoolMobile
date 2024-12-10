using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.ItemViewModels;
using School.Models;
using School.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace School.ViewModels
{
    public class SchoolMasterDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public SchoolMasterDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var mainViewModel = MainViewModel.GetInstance();

            
                List<Menu> menus = new List<Menu>
                {
                    new Menu
                    {
                        Icon = "ic_icon_myprofile.png",
                        PageName = $"{nameof(MyProfilePage)}",
                        Title = "My Profile",
                        IsLoginRequired = true,

                    },
                    new Menu
                    {
                        Icon = "ic_icon_evaluations.png",
                        PageName = $"{nameof(MyEvaluationsPage)}",
                        Title = "My Evaluations",
                        IsLoginRequired = true
                    },
                    new Menu
                    {
                        Icon = "ic_icon_weather.png",
                        PageName = $"{nameof(WeatherPage)}",
                        Title = "Weather",
                        IsLoginRequired = false
                    },
                    new Menu
                    {
                        Icon = "ic_icon_about.png",
                        PageName = $"{nameof(AboutPage)}",
                        Title = "About",
                        IsLoginRequired = false
                    }
                };

            

            if (mainViewModel.IsTokenValid() && mainViewModel.IsEmailSaved())
            {
                var menuItem = new Menu
                {
                    Icon = "ic_icon_logout.png",
                    PageName = $"{nameof(LogoutPage)}",
                    Title = "Logout",
                    IsLoginRequired = false
                };

                menus.Add(menuItem);
            }
            else
            {
                var menuItem = new Menu
                {
                    Icon = "ic_icon_login.png",
                    PageName = $"{nameof(LoginPage)}",
                    Title = "Login",
                    IsLoginRequired = false
                };

                menus.Add(menuItem);
            }

            Menus = new ObservableCollection<MenuItemViewModel>
                (menus.Select(x => new MenuItemViewModel(_navigationService)
                {
                    Icon = x.Icon,
                    PageName = x.PageName,
                    Title = x.Title,
                    IsLoginRequired = x.IsLoginRequired
                }).ToList());
        }

    }
}
