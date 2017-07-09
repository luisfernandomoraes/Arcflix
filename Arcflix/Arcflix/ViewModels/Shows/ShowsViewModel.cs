using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.NativeCallsInterfaces;
using Arcflix.Views.Shows;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Shows
{
    public class ShowsViewModel : BaseViewModel
    {
        #region Fields

        private int _pageIndex;
        private bool _isVisibleSearchBar;
        private readonly ShowsPage _showsPage;
        private string _filter;
        private IKeyboardInteractions _keyboardInteractions;
        private ObservableRangeCollection<ShowModel> _shows;

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
#pragma warning disable 618
                    Device.OnPlatform(Android: () => _keyboardInteractions.HideKeyboard());
#pragma warning restore 618
                    _showsPage.SearchBarShows.Unfocus();
                }
                else
                {
                    _showsPage.SearchBarShows.Focus();
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
        public ICommand FilterShowsCommand => new Command(async () => await FilterShowsAsync());

        private async Task FilterShowsAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {

                            IsBusy = true;
                            if (string.IsNullOrEmpty(Filter) && IsVisibleSearchBar)
                            {
#pragma warning disable 618
                                Device.OnPlatform(Android: () => _keyboardInteractions.HideKeyboard());
#pragma warning restore 618
                                _showsPage.SearchBarShows.Unfocus();
                                var items = ShowsBackup;
                                Shows.ReplaceRange(items);
                                Filter = string.Empty;
                                IsVisibleSearchBar = false;
                            }
                            else
                            {
                                List<ShowModel> filteredShows;
                                if (ShowsBackup.Count != 0)
                                    filteredShows = ShowsBackup.Where(x => x.Name.ToLower()
                                        .Contains(Filter.ToLower())).ToList();
                                else
                                    filteredShows = Shows.Where(x => x.Name.ToLower()
                                    .Contains(Filter.ToLower())).ToList();

                                Shows.ReplaceRange(filteredShows);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());
                            Task.Run(async () => await ExecuteLoadItemsCommand());
                        }
                        finally
                        {
                            IsBusy = false;
                        }
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
                Shows.Clear();
                _pageIndex = 1;
                var items = await ShowDataStore.GetItemsAsync(true);
                var shows = ShowModel.MovieListApiToMovieModelList(items);
                foreach (var showModel in shows)
                {
                    showModel.IsAdded = ShowDetailViewModel.IsShowSaved(showModel.IDApi);
                }
                if(ShowsBackup.Count == 0)
                    ShowsBackup.AddRange(shows);
                Shows.ReplaceRange(shows);
                Filter = string.Empty;
                IsVisibleSearchBar = false;
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

        public ObservableRangeCollection<ShowModel> Shows
        {
            get => _shows;
            set => SetProperty(ref _shows, value);
        }

        public List<ShowModel> ShowsBackup { get; set; }

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
                Task.Run(() => FilterShowsAsync());
            }
        }
        #endregion

        #region Constructor

        public ShowsViewModel(ShowsPage showsPage)
        {
            Title = "Popular TV Shows";
            Shows = new ObservableRangeCollection<ShowModel>();
            ShowsBackup = new List<ShowModel>();
            _pageIndex = 1;
            _showsPage = showsPage;
            _keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
        }

        #endregion

        #region Method

        public async Task LoadMore()
        {
            ++_pageIndex;
            var itens = await ShowDataStore.GetItemsAsync(true, _pageIndex);
            if (itens != null)
            {
                var enumerable = itens as Show[] ?? itens.ToArray();
                if (enumerable.Any())
                {
                    var shows = ShowModel.MovieListApiToMovieModelList(enumerable);
                    foreach (var showModel in shows)
                    {
                        showModel.IsAdded = ShowDetailViewModel.IsShowSaved(showModel.IDApi);
                    }
                    Shows.AddRange(shows);
                    ShowsBackup.AddRange(shows);
                }
            }
        }

        #endregion
    }
}