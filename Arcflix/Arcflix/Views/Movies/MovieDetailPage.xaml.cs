using System.Net.TMDb;
using Arcflix.Models;
using Arcflix.ViewModels;
using Arcflix.ViewModels.Movies;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        MovieDetailViewModel _viewModel;

        public MovieDetailPage(MovieModel movie)
        {
            InitializeComponent();
           BindingContext = _viewModel = new MovieDetailViewModel(movie);
        }
    }
}