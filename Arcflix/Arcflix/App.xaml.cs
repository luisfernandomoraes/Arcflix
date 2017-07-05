using System;
using System.Diagnostics;
using Arcflix.Services;
using Arcflix.Views;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Arcflix
{
    public partial class App : Application,ILoginManager
    {
        public static IToastNotificator Notificator;
        public static App Current;

        public App()
        {
            Current = this;
            InitializeComponent();
            LoadConfigurations();
            Current.MainPage = new NavigationPage(new LoginModalPage(this) { Title = "User Login" });
        }

        private void LoadConfigurations()
        {
            Properties["IsLoggedIn"] = false;
            
        }

        public static void SetMainPage()
        {
            Current.MainPage = new MasterDetailPage()
            {
                Master = new MasterPage() { Title = "Main Page" },
                Detail = new NavigationPage(new ItemsPage())
            };
        }

        /// <summary>
        /// Login Partner Implementation 
        /// </summary>
        public void ShowMainPage()
        {
            SetMainPage();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method to show a toast notification 
        /// </summary>
        /// <param name="toastNotificationType">A type of a notification.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="seconds">The time duration of notification.</param>
        public static void ShowToast(ToastNotificationType toastNotificationType, string title, string message, int? seconds = null)
        {
            try
            {
                Notificator = Notificator ?? DependencyService.Get<IToastNotificator>();
                Notificator.Notify(toastNotificationType, title, message, seconds != null ? TimeSpan.FromSeconds((int)seconds) : TimeSpan.FromSeconds(3));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
