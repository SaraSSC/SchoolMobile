using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace School.Views
{
    public partial class LogoutPage : ContentPage
    {
      
        public LogoutPage()
        {
            InitializeComponent();
            BtnLogout.Animate("Animation", new Animation(Animation()) );
           
            
        }

       
       
        public  System.Action<double> Animation()
        {
            uint timeout = 1;
            BtnLogout.TranslateTo(-15, 0, timeout);
            BtnLogout.TranslateTo(15, 0, timeout);
            BtnLogout.TranslateTo(-10, 0, timeout);
            BtnLogout.TranslateTo(10, 0, timeout);
            BtnLogout.TranslateTo(-5, 0, timeout);
            BtnLogout.TranslateTo(5, 0, timeout);
            BtnLogout.TranslationX = 0;

            return TranslationX => BtnLogout.TranslationX = TranslationX;
        }
    }
}

