﻿using System;
using System.Diagnostics;
using System.Net.TMDb;
using System.Threading.Tasks;

using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.Views;

using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Movie> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private int _pageIndex;
        public ItemsViewModel()
        {
            Title = "Upcoming Movies";
            Items = new ObservableRangeCollection<Movie>();
            _pageIndex = 1;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Movie>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Movie;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        public async Task LoadMore()
        {
            ++_pageIndex;
            var itens = await DataStore.GetItemsAsync(true,_pageIndex);
            Items.AddRange(itens);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
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