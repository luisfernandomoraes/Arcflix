using System.Net.TMDb;

namespace Arcflix.ViewModels
{
    public class MovieDetailViewModel:BaseViewModel
    {
        public Movie MovieDetail { get; private set; }
        public MovieDetailViewModel(Movie movie)
        {
            Title = "Movie Details";
            MovieDetail = movie;
        }
    }
}