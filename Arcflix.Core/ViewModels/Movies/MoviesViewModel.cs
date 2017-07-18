using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.NativeCallsInterfaces;
using Arcflix.Views;
using Arcflix.Views.Movies;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Movies
{
    public class MoviesViewModel : BaseViewModel
    {
        #region Fields

        private int _pageIndex;
        private int _pageIndexSearch;
        private bool _isVisibleSearchBar;
        private readonly MoviesPage _moviesPage;
        private string _filter;
        private IKeyboardInteractions _keyboardInteractions;
        private ObservableRangeCollection<MovieModel> _movies;
        private ObservableRangeCollection<MovieModel> _moviesBackup;


        #endregion

        #region Commands
        public ICommand RequestSearchCommand => new Command(RequestSearch);
        private async void RequestSearch(object obj)
        {
            try
            {
                IsVisibleSearchBar = !IsVisibleSearchBar;
                if (!IsVisibleSearchBar)
                {
                    Filter = string.Empty;
                    Device.BeginInvokeOnMainThread(() =>
                    {
#pragma warning disable 618
                        Device.OnPlatform(Android: () => _keyboardInteractions.HideKeyboard());
#pragma warning restore 618
                        _moviesPage.SearchBarMovies.Unfocus();
                    });
                    Movies.ReplaceRange(MoviesBackup);

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
        public ICommand FilterMoviesCommand => new Command(FilterMoviesAsync);

        private async void FilterMoviesAsync()
        {

            try
            {

                IsBusy = true;
                if (string.IsNullOrEmpty(Filter) && IsVisibleSearchBar)
                {
#pragma warning disable 618
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            Device.OnPlatform(Android: () => _keyboardInteractions.HideKeyboard());
                            _moviesPage.SearchBarMovies.Unfocus();
                        });
#pragma warning restore 618
                    await ExecuteLoadItemsCommand();
                    Filter = string.Empty;
                    IsVisibleSearchBar = false;
                }
                else
                {
                    var filteredMovies = MovieModel.MovieListApiToMovieModelList(await MovieDataStore.GetSearchResult(Filter)).ToList();
                    Movies.ReplaceRange(filteredMovies);

                    Device.BeginInvokeOnMainThread(()=>_moviesPage.MoviesListView.ScrollTo(_moviesPage.MoviesListView.ItemsSource.Cast<object>().First(), ScrollToPosition.Start, false));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                await ExecuteLoadItemsCommand();
            }
            finally
            {
                IsBusy = false;
            }

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
                MoviesBackup.Clear();

                _pageIndex = 1;
                var items = await MovieDataStore.GetItemsAsync(true);
                var movies = MovieModel.MovieListApiToMovieModelList(items);
                foreach (var movieModel in movies)
                {
                    movieModel.IsAdded = MovieDetailViewModel.IsMovieSaved(movieModel.IDApi);
                }

                MoviesBackup.AddRange(movies);
                Movies.AddRange(movies);
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

        public ObservableRangeCollection<MovieModel> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public ObservableRangeCollection<MovieModel> MoviesBackup
        {
            get => _moviesBackup;
            set => SetProperty(ref _moviesBackup, value);
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
                OnPropertyChanged();
                if (string.IsNullOrEmpty(value)) return;
                _pageIndexSearch = 0;
                Task.Run(() => FilterMoviesAsync());
            }
        }
        #endregion

        #region Constructor

        public MoviesViewModel(MoviesPage moviesPage)
        {
            Title = "Upcoming Movies";
            Movies = new ObservableRangeCollection<MovieModel>();
            MoviesBackup = new ObservableRangeCollection<MovieModel>();
            _pageIndex = 1;
            _moviesPage = moviesPage;
            _keyboardInteractions = DependencyService.Get<IKeyboardInteractions>();
        }

        #endregion

        #region Methods

        public async Task LoadMore()
        {
            ++_pageIndex;
            var itens = await MovieDataStore.GetItemsAsync(true, _pageIndex);
            if (itens != null)
            {
                var enumerable = itens as Movie[] ?? itens.ToArray();
                if (enumerable.Any())
                {
                    var movies = MovieModel.MovieListApiToMovieModelList(enumerable);
                    foreach (var movieModel in movies)
                    {
                        movieModel.IsAdded = MovieDetailViewModel.IsMovieSaved(movieModel.IDApi);
                    }
                    Movies.AddRange(movies);
                    MoviesBackup.AddRange(movies);
                }
            }
        }

        public async Task LoadMoreSearchResults()
        {
            ++_pageIndexSearch;
            var itens = await MovieDataStore.GetSearchResult(Filter, _pageIndexSearch);
            if (itens != null)
            {
                var enumerable = itens as Movie[] ?? itens.ToArray();
                if (enumerable.Any())
                {
                    var movies = MovieModel.MovieListApiToMovieModelList(enumerable);
                    foreach (var movieModel in movies)
                    {
                        movieModel.IsAdded = MovieDetailViewModel.IsMovieSaved(movieModel.IDApi);
                    }

                    Movies.AddRange(movies);
                }
            }
        }

        #endregion
    }
}