using System.Threading.Tasks;
using Xamarin.Forms;

namespace School.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BtnLogin.Clicked += async (sender, args) => await FadeAnimation();
        }

        public async Task FadeAnimation()
        {
            BtnLogin.Opacity = 1;

            // Fade out
            await BtnLogin.FadeTo(0, 1000);
            // Fade in
            await BtnLogin.FadeTo(1, 1000);
        }
    }
}
