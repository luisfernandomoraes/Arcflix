using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using Arcflix.Helpers;
using Arcflix.Services.Api;
using Xamarin.Forms;

[assembly: Dependency(typeof(Arcflix.Services.ShowDataStore))]
namespace Arcflix.Services
{
    public class ShowDataStore:IDataStore<Show>
    {
        private bool _isInitialized;
        private List<Show> _shows;
        private ServiceClient _clientTmDb;
        private TMDBExtendedApi _tmdbExtendedApi;


        public async Task<bool> AddItemAsync(Show item)
        {
            await InitializeAsync();

            _shows.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Show item)
        {
            await InitializeAsync();

            var show = _shows.FirstOrDefault(arg => arg.Id == item.Id);
            _shows.Remove(show);
            _shows.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Show item)
        {
            await InitializeAsync();

            var show = _shows.FirstOrDefault(arg => arg.Id == item.Id);
            _shows.Remove(show);

            return await Task.FromResult(true);
        }

        public async Task<Show> GetItemAsync(int id, bool isGetDetails = false)
        {
            await InitializeAsync();

            _tmdbExtendedApi = _tmdbExtendedApi ?? new TMDBExtendedApi(Settings.ApiKey);
            var shows = await _tmdbExtendedApi.GetShowDetailsAsync(id, Settings.Language);
            return await Task.FromResult(shows);
        }

        public async Task<IEnumerable<Show>> GetItemsAsync(bool forceRefresh = false, int pageIndex = 1)
        {
            await InitializeAsync();

            var shows = await GetItemsByPage(pageIndex);

            return await Task.FromResult(shows.Results);
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _shows = new List<Show>();
            var shows = await GetItemsByPage();
            foreach (Show show in shows.Results)
            {
                _shows.Add(show);
            }
            _isInitialized = true;
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }
        private async Task<Shows> GetItemsByPage(int index = 1)
        {

            _clientTmDb = _clientTmDb ?? new ServiceClient(Settings.ApiKey);
            Shows shows = await _clientTmDb.Shows.GetPopularAsync(Settings.Language, index, new CancellationToken());
            return await Task.FromResult(shows);
        }
    }
}