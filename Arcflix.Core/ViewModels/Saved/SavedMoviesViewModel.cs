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
using Arcflix.Views;
using Arcflix.Views.Movies;
using Arcflix.Views.Saved;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Saved
{
    public class SavedMoviesViewModel : BaseViewModel
    {
        #region Fields

        private int _pageIndex;
        private bool _isVisibleSearchBar;
        private readonly SavedMoviesPage _moviesPage;
        private string _filter;
        private IKeyboardInteractions _keyboardInteractions;
        private ObservableRangeCollection<MovieModel> _movies;

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
        public ICommand FilterMoviesCommand => new Command(async () => await FilterMoviesAsync());

        private async Task FilterMoviesAsync()
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
                        _moviesPage.SearchBarMovies.Unfocus();
                        await ExecuteLoadItemsCommand();
                        Filter = string.Empty;
                        IsVisibleSearchBar = false;
                    }
                    else
                    {
                        var filteredMovies = MoviesBackup.Where(x => x.Title.ToLower()
                            .Contains(Filter.ToLower())).ToList();

                        Movies.ReplaceRange(filteredMovies);
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
                Movies.Clear();
                _pageIndex = 1;
                var movies = ArcflixDBContext.MovieDataBase.GetItems();
                foreach (var movieModel in movies)
                {
                    movieModel.IsAdded = true;
                }
                MoviesBackup = movies;
                Movies.ReplaceRange(movies);
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

        public IEnumerable<MovieModel> MoviesBackup { get; set; }

        #endregion

        #region Properties

        public ObservableRangeCollection<MovieModel> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }


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
                Task.Run(() => FilterMoviesAsync());
            }
        }
        #endregion

        #region Constructor

        public SavedMoviesViewModel(SavedMoviesPage moviesPage)
        {
            Title = "Upcoming Movies";
            Movies = new ObservableRangeCollection<MovieModel>();
            _pageIndex = 1;
            _moviesPage = moviesPage;
            _keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
            MessagingCenter.Subscribe<SavedMovieDetailViewModel>(this, "ItemChanged", async (args) =>
            {
                await ExecuteLoadItemsCommand();
            });
        }

        #endregion

    }
}