using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;

namespace Arcflix.Controls
{
    public class FacebookEventArgs : EventArgs
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }

    public class FacebookButton : Button
    {
        public Action<object, FacebookEventArgs> OnLogin;
        public void Login(object sender, FacebookEventArgs args)
        {
            IsVisible = false;
            OnLogin?.Invoke(sender, args);
        }

    }
}