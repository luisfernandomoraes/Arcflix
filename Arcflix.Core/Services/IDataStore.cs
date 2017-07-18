using System.Collections.Generic;
using System.Net.TMDb;
using System.Threading.Tasks;

namespace Arcflix.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<IEnumerable<Video>> GetItemVideoAsync(int id);
        Task<T> GetItemAsync(int id, bool isGetDetails = false);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false, int pageIndex = 1);
        Task<IEnumerable<T>> GetSearchResult(string searchText, int pageIndex = 1);
        Task InitializeAsync();
        Task<bool> PullLatestAsync();
        Task<bool> SyncAsync();
    }
}
