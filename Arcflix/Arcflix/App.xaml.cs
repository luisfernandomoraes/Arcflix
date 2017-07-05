using Arcflix.Services;
using Arcflix.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Arcflix
{
    public partial class App : Application,ILoginManager
    {
        public App()
        {
            InitializeComponent();

            Current.MainPage = new NavigationPage(new LoginModalPage(this) { Title = "User Login" });
        }

        public static void SetMainPage()
        {
            Current.MainPage = new MasterDetailPage()
            {
                Master = new MasterPage() { Title = "Main Page" },
                Detail = new NavigationPage(new ItemsPage())
            };
        }

        public void ShowMainPage()
        {
            SetMainPage();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}
