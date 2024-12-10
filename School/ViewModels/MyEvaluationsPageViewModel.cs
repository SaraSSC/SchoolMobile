using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using School.ItemViewModels;
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
    public class MyEvaluationsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;

        private bool _isRunning;
        private string _search;
        private string _noCoursesFound;


        private DelegateCommand _searchCommand;
        private ObservableCollection<CourseItemViewModel> _courses;
        private List<CourseResponse> _myCourses;
        public MyEvaluationsPageViewModel(INavigationService navigationService, ApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            LoadCoursesAsync();
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public DelegateCommand SearchCommand => _searchCommand
            ?? (_searchCommand = new DelegateCommand(ShowCourses));

        public ObservableCollection<CourseItemViewModel> Courses
        {
            get => _courses;
            set => SetProperty(ref _courses, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowCourses();
            }
        }

        public string NoCoursesFound
        {
            get => _noCoursesFound;
            set => SetProperty(ref _noCoursesFound, value);
        }

        private async void LoadCoursesAsync()
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
            string controller = App.Current.Resources["ApiSchoolStudentCourses"].ToString() + $"/{mainViewModel.Email}";

            Response response = await _apiService.GetMultipleResultsAsync<CourseResponse>
                (url, prefix, controller, "bearer", MainViewModel.GetInstance().Token.Token);

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Something went wrong:", response.Message, "Ok");

                return;
            }

            _myCourses = (response.Result as List<CourseResponse>).OrderBy(x => x.Name).ToList();
            ShowCourses();
        }

        private void ShowCourses()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Courses = new ObservableCollection<CourseItemViewModel>(_myCourses.Select(c => new CourseItemViewModel(_navigationService)
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Area = c.Area,
                    Duration = c.Duration,

                }).ToList());
                return;
            }
            else
            {
                Courses = new ObservableCollection<CourseItemViewModel>
                   (_myCourses.Select(x => new CourseItemViewModel(_navigationService)
                   {
                       Id = x.Id,
                       Code = x.Code,
                       Name = x.Name,
                       Area = x.Area,
                       Duration = x.Duration
                   })
                   .Where(x => x.Name.ToLower().Contains(Search.ToLower()) || x.Area.ToLower().Contains(Search.ToLower()))
                   .ToList());
            }

            if (Courses is null || Courses.Count <= 0)
            {
                NoCoursesFound = "No courses found";
            }
        }

    }
}
