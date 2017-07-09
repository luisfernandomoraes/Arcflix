using System.Net.TMDb;
using Arcflix.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        MovieDetailViewModel _viewModel;

        public MovieDetailPage(Movie movie)
        {
            InitializeComponent();
           BindingContext = _viewModel = new MovieDetailViewModel(movie);
        }
    }
}