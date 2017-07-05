using Arcflix.Views;
using Xamarin.Forms;

namespace Arcflix.Services
{
    public class LoginModalPage : CarouselPage
    {
        readonly ContentPage _login;

        public LoginModalPage(ILoginManager ilm)
        {
            if (!(bool)Application.Current.Properties["IsLoggedIn"])
            {
                _login = new LoginPage(ilm);
            }

            Children.Add(_login);

            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
            {
                SelectedItem = _login;
            });

        }
    }
}