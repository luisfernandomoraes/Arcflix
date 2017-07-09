using System.Net.TMDb;
using Arcflix.Models;
using Arcflix.ViewModels;
using Arcflix.ViewModels.Movies;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Saved
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedMovieDetailPage : ContentPage
    {
        MovieDetailViewModel _viewModel;

        public SavedMovieDetailPage(MovieModel movie)
        {
            InitializeComponent();
           BindingContext = _viewModel = new MovieDetailViewModel(movie);
        }
    }
}