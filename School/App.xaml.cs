using Prism;
using Prism.Ioc;
using School.ViewModels;
using School.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace School
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXZcdHVTR2BeVkBzWUI=");


            await NavigationService.NavigateAsync($"NavigationPage/{nameof(LoginPage)}");
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SchoolMasterDetailsPage, SchoolMasterDetailsPageViewModel>();

            containerRegistry.RegisterForNavigation<MyProfilePage, MyProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<LogoutPage, LogoutPageViewModel>();



            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<MyEvaluationsPage, MyEvaluationsPageViewModel>();
            containerRegistry.RegisterForNavigation<MyEvaluationDetailsPage, MyEvaluationDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<WeatherDetailsPage, WeatherDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<WeatherPage, WeatherPageViewModel>();
        }
    }
}
