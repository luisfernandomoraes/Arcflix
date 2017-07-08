using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.Views;

using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        #region Fields

        private int _pageIndex;
        private bool _isVisibleSearchBar;
        private readonly MoviesPage _moviesPage;
        private string _filter;
        private bool _canShowListView;
        private bool _canShowFilterListView;

        #endregion

        #region Commands
        public ICommand RequestSearchCommand => new Command(RequestSearch);
        private void RequestSearch(object obj)
        {
            try
            {
                IsVisibleSearchBar = !IsVisibleSearchBar;
                if (!IsVisibleSearchBar)
                {
                    Filter = string.Empty;
                    CanShowListView = true;
                    CanShowFilterListView = false;
                    //Device.OnPlatform(Android: () => _hidingKeyboard.HideKeyboard());
                    _moviesPage.SearchBarMovies.Unfocus();
                }
                else
                {
                    _moviesPage.SearchBarMovies.Focus();
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
        public ICommand FilterPromotionsCommand => new Command(async () => await FilterPromotionsAsync());

        private async Task FilterPromotionsAsync()
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                if (string.IsNullOrEmpty(Filter) && IsVisibleSearchBar)
                {
                    CanShowListView = true;
                    CanShowFilterListView = false;

                    //Device.OnPlatform(Android: () => _hidingKeyboard.HideKeyboard());
                    _moviesPage.SearchBarMovies.Unfocus();
                    IsVisibleSearchBar = false;

                }
                else
                {
                    CanShowListView = false;
                    CanShowFilterListView = true;
                }

                /*CatalogPricesFiltered =
                    new ObservableCollection<CatalogPrice>(
                        CatalogPrices.Where(x => x.ProductName.ToLower().Contains(Filter.ToLower())));*/


                IsBusy = false;
            });
        }

        public ICommand LoadItemsCommand => new Command(async () => await ExecuteLoadItemsCommand());
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Movies.Clear();
                _pageIndex = 1;
                var items = await MovieDataStore.GetItemsAsync(true);
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

        #endregion

        #region Properties

        public ObservableRangeCollection<Movie> Movies { get; set; }
        public bool IsVisibleSearchBar
        {
            get => _isVisibleSearchBar;
            set => SetProperty(ref _isVisibleSearchBar, value);
        }
        public string Filter
        {
            get => _filter;
            set
            {
                if (value == _filter) return;
                _filter = value;
                Task.Run(() => FilterPromotionsAsync());
            }
        }
        public bool CanShowListView
        {
            get => _canShowListView;
            set => SetProperty(ref _canShowListView,value); 
        }

        public bool CanShowFilterListView
        {
            get => _canShowFilterListView;
            set => SetProperty(ref _canShowFilterListView,value);
        }
        #endregion

        #region Constructor

        public MoviesViewModel(MoviesPage moviesPage)
        {
            Title = "Upcoming Movies";
            Movies = new ObservableRangeCollection<Movie>();
            _pageIndex = 1;
            _moviesPage = moviesPage;
        }

        #endregion

        public async Task LoadMore()
        {
            ++_pageIndex;
            var itens = await MovieDataStore.GetItemsAsync(true, _pageIndex);
            if (itens != null)
            {
                var enumerable = itens as Movie[] ?? itens.ToArray();
                if (enumerable.Any())
                    Movies.AddRange(enumerable);
            }
        }

        
    }
}