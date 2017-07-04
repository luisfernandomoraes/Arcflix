using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;

using Arcflix.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Arcflix.Services.MockDataStore))]
namespace Arcflix.Services
{
    public class MockDataStore : IDataStore<Movie>
    {
        private bool isInitialized;
        private List<Movie> Movies;
        private ServiceClient _clientTMDb;
        public async Task<bool> AddItemAsync(Movie item)
        {
            await InitializeAsync();

            Movies.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Movie item)
        {
            await InitializeAsync();

            var movie = Movies.FirstOrDefault(arg => arg.Id == item.Id);
            Movies.Remove(movie);
            Movies.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Movie item)
        {
            await InitializeAsync();

            var movie = Movies.FirstOrDefault(arg => arg.Id == item.Id);
            Movies.Remove(movie);

            return await Task.FromResult(true);
        }

        public async Task<Movie> GetItemAsync(int id)
        {
            await InitializeAsync();

            return await Task.FromResult(Movies.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Movie>> GetItemsAsync(bool forceRefresh = false, int pageIndex = 1)
        {
            if (!isInitialized)
                await InitializeAsync();
            else
                await GetItemsByPage(pageIndex);

            return await Task.FromResult(Movies);
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
            if (isInitialized)
                return;

            Movies = new List<Movie>();
            await GetItemsByPage();

            isInitialized = true;
        }

        private async Task GetItemsByPage(int index = 1)
        {
            _clientTMDb = _clientTMDb ?? new ServiceClient("1f54bd990f1cdfb230adb312546d765d");
            Movies movies = await _clientTMDb.Movies.GetUpcomingAsync("en-US", index, new CancellationToken());

            foreach (Movie movie in movies.Results)
            {
                Movies.Add(movie);
            }
        }
    }
}
