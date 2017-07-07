using System.Net.TMDb;

namespace Arcflix.ViewModels
{
    public class MovieDetailViewModel:BaseViewModel
    {
        public Movie MovieDetail { get; set; }
        public MovieDetailViewModel(Movie movie)
        {
            MovieDetail = movie;
        }
    }
}