using System.Net.TMDb;
using Arcflix.ViewModels.Shows;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Shows
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowDetailPage : ContentPage
    {
        ShowDetailViewModel _viewModel;

        public ShowDetailPage(Show show)
        {
            InitializeComponent();
           BindingContext = _viewModel = new ShowDetailViewModel(show);
        }
    }
}