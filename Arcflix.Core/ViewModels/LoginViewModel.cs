using System.Windows.Input;
using Arcflix.Services;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class LoginViewModel
    {

        #region Fields

        private ILoginManager _ilm;
        private IToastNotificator _notificator;

        #endregion

        #region Commands

        public ICommand LoginWithoutFacebookCommand => new Command(LoginWithoutFacebook);

        private void LoginWithoutFacebook(object obj)
        {
            Application.Current.Properties["IsLoggedIn"] = true;
            App.Current.ShowMainPage();
        }

        #endregion

        #region Constructors

        public LoginViewModel(ILoginManager ilm)
        {
            _ilm = ilm;
            _notificator = DependencyService.Get<IToastNotificator>();
        }


        #endregion
    }
}