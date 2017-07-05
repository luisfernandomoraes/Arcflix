using System.Collections.Generic;
using System.Threading.Tasks;
using Arcflix.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Arcflix.Services.UserDataStore))]
namespace Arcflix.Services
{
    public class UserDataStore: IDataStore<User>
    {
        public Task<bool> AddItemAsync(User item)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(User item)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(User item)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetItemAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false, int pageIndex = 1)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> PullLatestAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SyncAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}