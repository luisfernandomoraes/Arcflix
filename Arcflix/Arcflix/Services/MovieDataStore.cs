using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using Arcflix.Helpers;
using Arcflix.Services.Api;
using Xamarin.Forms;

[assembly: Dependency(typeof(Arcflix.Services.MovieDataStore))]
namespace Arcflix.Services
{
    public class MovieDataStore : IDataStore<Movie>
    {
        private bool _isInitialized;
        private List<Movie> _movies;
        private ServiceClient _clientTmDb;
        private TMDBExtendedApi _tmdbExtendedApi;
        public async Task<bool> AddItemAsync(Movie item)
        {
            await InitializeAsync();

            _movies.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Movie item)
        {
            await InitializeAsync();

            var movie = _movies.FirstOrDefault(arg => arg.Id == item.Id);
            _movies.Remove(movie);
            _movies.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Movie item)
        {
            await InitializeAsync();

            var movie = _movies.FirstOrDefault(arg => arg.Id == item.Id);
            _movies.Remove(movie);

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Video>> GetItemVideoAsync(int id)
        {
            var videos = await _clientTmDb.Movies.GetVideosAsync(id, Settings.Language, new CancellationToken());
            return videos;
        }

        public async Task<Movie> GetItemAsync(int id, bool isGetDetails = false)
        {
            await InitializeAsync();
            if (!isGetDetails)
                return await Task.FromResult(_movies.FirstOrDefault(s => s.Id == id));
            return await GetItemDetailAsync(id);
        }
        public async Task<Movie> GetItemDetailAsync(int id)
        {
            await InitializeAsync();

            _tmdbExtendedApi = _tmdbExtendedApi ?? new TMDBExtendedApi("1f54bd990f1cdfb230adb312546d765d");
            var movie = await _tmdbExtendedApi.GetMovieDetailsAsync(id, "en-US");
            return await Task.FromResult(movie);
        }

        public async Task<IEnumerable<Movie>> GetItemsAsync(bool forceRefresh = false, int pageIndex = 1)
        {
            await InitializeAsync();

            var movies = await GetItemsByPage(pageIndex);

            return await Task.FromResult(movies.Results);

        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _movies = new List<Movie>();
            var movies = await GetItemsByPage();
            foreach (Movie movie in movies.Results)
            {
                _movies.Add(movie);
            }
            _isInitialized = true;
        }

        private async Task<Movies> GetItemsByPage(int index = 1)
        {

            _clientTmDb = _clientTmDb ?? new ServiceClient("1f54bd990f1cdfb230adb312546d765d");
            Movies movies = await _clientTmDb.Movies.GetUpcomingAsync(Settings.Language, index, new CancellationToken());
            return await Task.FromResult(movies);
        }
    }
}
