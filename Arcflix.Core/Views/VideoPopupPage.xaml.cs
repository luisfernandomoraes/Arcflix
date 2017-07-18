using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcflix.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPopupPage : PopupPage
    {
        private VideoPopupViewModel _viewModel;

        public VideoPopupPage(string youtubeKey)
        {
            InitializeComponent();
            BindingContext = _viewModel = new VideoPopupViewModel(youtubeKey);
        }
    }
}