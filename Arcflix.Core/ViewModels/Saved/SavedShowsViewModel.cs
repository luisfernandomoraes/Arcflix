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
using Arcflix.Services.DB;
using Arcflix.Views.Saved;
using Arcflix.Views.Shows;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Saved
{
    public class SavedShowsViewModel : BaseViewModel
    {
        #region Fields

        private int _pageIndex;
        private bool _isVisibleSearchBar;
        private readonly SavedShowsPage _showsPage;
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
            Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {

                            if (string.IsNullOrEmpty(Filter) && IsVisibleSearchBar)
                            {
#pragma warning disable 618
                                Device.OnPlatform(Android: () => _keyboardInteractions.HideKeyboard());
#pragma warning restore 618
                                _showsPage.SearchBarShows.Unfocus();
                                await ExecuteLoadItemsCommand();
                                Filter = string.Empty;
                                IsVisibleSearchBar = false;
                            }
                            else
                            {
                                var filteredShows = ShowsBackup.Where(x => x.Name.ToLower()
                                    .Contains(Filter.ToLower())).ToList();

                                Shows.ReplaceRange(filteredShows);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());
                            Task.Run(async () => await ExecuteLoadItemsCommand());
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
                var shows = ArcflixDBContext.ShowDataBase.GetItems();
                foreach (var showModel in shows)
                {
                    showModel.IsAdded = true;
                }
                ShowsBackup = shows;
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

        public IEnumerable<ShowModel> ShowsBackup { get; private set; }

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

        public SavedShowsViewModel(SavedShowsPage showsPage)
        {
            Title = "Popular TV Shows";
            Shows = new ObservableRangeCollection<ShowModel>();
            _pageIndex = 1;
            _showsPage = showsPage;
            _keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
            MessagingCenter.Subscribe<SavedShowDetailViewModel>(this, "ItemChanged", async (args) =>
            {
                await ExecuteLoadItemsCommand();
            });
        }

        #endregion
    }
}