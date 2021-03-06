﻿using System;
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
        private bool _isVisibleSearchBar;
        private readonly MoviesPage _moviesPage;
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
                                _moviesPage.SearchBarMovies.Unfocus();
                                var items = MoviesBackup;
                                Movies.ReplaceRange(items);
                                Filter = string.Empty;
                                IsVisibleSearchBar = false;
                            }
                            else
                            {
                                List<MovieModel> filteredMovies;
                                if (MoviesBackup.Count != 0)
                                    filteredMovies = MoviesBackup.Where(x => x.Title.ToLower()
                                        .Contains(Filter.ToLower())).ToList();
                                else
                                    filteredMovies = Movies.Where(x => x.Title.ToLower()
                                    .Contains(Filter.ToLower())).ToList();

                                Movies.ReplaceRange(filteredMovies);
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
                Movies.Clear();
                _pageIndex = 1;
                var items = await MovieDataStore.GetItemsAsync(true);
                var movies = MovieModel.MovieListApiToMovieModelList(items);
                foreach (var movieModel in movies)
                {
                    movieModel.IsAdded = MovieDetailViewModel.IsMovieSaved(movieModel.IDApi);
                }
                if (MoviesBackup.Count == 0)
                    MoviesBackup.AddRange(movies);
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

        #endregion

        #region Properties

        public ObservableRangeCollection<MovieModel> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public List<MovieModel> MoviesBackup { get; set; }

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

        public MoviesViewModel(MoviesPage moviesPage)
        {
            Title = "Upcoming Movies";
            Movies = new ObservableRangeCollection<MovieModel>();
            MoviesBackup = new List<MovieModel>();
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

       

        #endregion
    }
}