using System.Net.TMDb;
using System.Threading.Tasks;

namespace Arcflix.Services.Api
{
    public interface ITMDBExtendedApi
    {
        Task<Movie> GetMovieDetailsAsync(int movieId, string language);
    }
}