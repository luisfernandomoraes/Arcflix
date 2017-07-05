using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcflix.Services;
using Arcflix.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage(ILoginManager ilm)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(ilm);
        }
    }
}