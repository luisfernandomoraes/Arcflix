using System;
using System.Diagnostics;
using System.Linq;
using Arcflix.Services;
using Arcflix.Services.DB;
using Arcflix.Views;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Arcflix
{
    public partial class App : Application, ILoginManager
    {
        public static IToastNotificator Notificator;
        public static App Current;
        public bool IsLoggedIn;

        public App()
        {
            Current = this;
            InitializeComponent();
            LoadConfigurations();
            SetMainPage();
        }

        private void LoadConfigurations()
        {
            Properties["IsLoggedIn"] = false;
            var user = ArcflixDBContext.UserDataBase.GetItems().FirstOrDefault();
            if (user != null)
                Properties["IsLoggedIn"] = true;

        }

        public void SetMainPage()
        {
            IsLoggedIn = Properties.ContainsKey(nameof(IsLoggedIn)) && Convert.ToBoolean(Properties[nameof(IsLoggedIn)]);
            if (!IsLoggedIn)
                Current.MainPage = new NavigationPage(new LoginModalPage(this) { Title = "You can login with your Facebook!" });
            else
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
        /// <summary>
        /// Metodo usado para ocultar o page que faz o login via facebook. 
        /// </summary>
        public static Action HideLoginView
        {
            get
            {
                return () => Current.MainPage.Navigation.PopModalAsync();
            }
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
