using System.Net.TMDb;
using System.Threading.Tasks;

namespace Arcflix.Services.TMDbApi
{
    public interface ITMDBApi
    {
        Task<Movie> GetUpcomingMovies(string apiKey,string language = "en-US",int page = 1);
    }
}