using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Text;
using System.Threading.Tasks;
using Arcflix.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views
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