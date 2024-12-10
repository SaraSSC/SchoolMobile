using Prism.Commands;
using Prism.Navigation;
using School.ViewModels;
using School.Views;
using System;
using System.Collections.Generic;
using System.Text;
using School.Models;
namespace School.ItemViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ??
            (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (IsLoginRequired)
            {
                var mainViewModel = MainViewModel.GetInstance();

                if (mainViewModel.Token == null)
                {
                    await _navigationService.NavigateAsync
                        ($"/{nameof(SchoolMasterDetailsPage)}/NavigationPage/{nameof(LoginPage)}");
                    return;
                }

                if (!mainViewModel.IsTokenValid())
                {
                    await _navigationService.NavigateAsync
                        ($"/{nameof(SchoolMasterDetailsPage)}/NavigationPage/{nameof(LoginPage)}");
                    return;
                }
            }

            // Navigate to the selected page
            await _navigationService.NavigateAsync
                ($"/{nameof(SchoolMasterDetailsPage)}/NavigationPage/{PageName}");
        }
    }
}
