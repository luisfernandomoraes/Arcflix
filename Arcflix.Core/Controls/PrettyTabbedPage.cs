using Xamarin.Forms;

namespace Arcflix.Controls
{
    public class PrettyTabbedPage : TabbedPage
    {

        public static readonly BindableProperty ShowTitlesProperty =
            BindableProperty.Create("ShowTitles", typeof(bool), typeof(PrettyTabbedPage), true);

        public bool ShowTitles
        {
            get { return (bool)GetValue(ShowTitlesProperty); }
            set { SetValue(ShowTitlesProperty, value); }
        }
    }
}