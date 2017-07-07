using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;

using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.Views;

using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Movie> Movies { get; set; }
        public Command LoadItemsCommand { get; set; }
        private int _pageIndex;
        public MoviesViewModel()
        {
            Title = "Upcoming Movies";
            Movies = new ObservableRangeCollection<Movie>();
            _pageIndex = 1;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Movie>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Movie;
                Movies.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        public async Task LoadMore()
        {
            ++_pageIndex;
            var itens = await DataStore.GetItemsAsync(true, _pageIndex);
            if (itens != null)
            {
                var enumerable = itens as Movie[] ?? itens.ToArray();
                if (enumerable.Any())
                    Movies.AddRange(enumerable);
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                _pageIndex = 1;
                var items = await DataStore.GetItemsAsync(true);
                Movies.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}