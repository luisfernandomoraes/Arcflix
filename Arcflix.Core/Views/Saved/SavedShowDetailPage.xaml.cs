using System.Net.TMDb;
using Arcflix.Models;
using Arcflix.ViewModels.Saved;
using Arcflix.ViewModels.Shows;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Saved
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedShowDetailPage : ContentPage
    {
        SavedShowDetailViewModel _viewModel;

        public SavedShowDetailPage(ShowModel show)
        {
            InitializeComponent();
           BindingContext = _viewModel = new SavedShowDetailViewModel(show);
        }
    }
}