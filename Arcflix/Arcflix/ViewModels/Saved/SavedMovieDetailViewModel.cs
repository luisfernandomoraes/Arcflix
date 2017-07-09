﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Models;
using Arcflix.Services;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Arcflix.ViewModels.Saved
{
    public class SavedMovieDetailViewModel : BaseViewModel
    {
        #region Fields

        private string _genders;
        private string _toolBarItemIcon;

        #endregion

        #region Commands

        public ICommand SaveMovieCommand => new Command(SaveMovie);

        private void SaveMovie(object obj)
        {
            try
            {
                var movieFromDB = Arcflix.Services.DB.ArcflixDBContext.MovieDataBase.GetItems()
                    .FirstOrDefault(x => x.IDApi == MovieDetail.Id);
                if (movieFromDB != null)
                {
                    ToolBarItemIcon = "ic_bookmark_24dp.png";
                    Arcflix.Services.DB.ArcflixDBContext.MovieDataBase.DeleteItem(movieFromDB.ID);
                    App.ShowToast(ToastNotificationType.Success, "Arcflix", "movie removed!", 3);

                }
                else
                {
                    ToolBarItemIcon = "ic_bookmark_white_24dp.png";
                    var movieModel = MovieModel.MovieApiToMovieModel(MovieDetail);
                    Arcflix.Services.DB.ArcflixDBContext.MovieDataBase.SaveItem(movieModel);
                    App.ShowToast(ToastNotificationType.Success, "Arcflix", "movie saved!", 3);
                    
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Properties

        public Movie MovieDetail { get; private set; }

        public string ToolBarItemIcon
        {
            get => _toolBarItemIcon;
            set => SetProperty(ref _toolBarItemIcon, value);
        }

        public string Genders
        {
            get => _genders;
            set => SetProperty(ref _genders, value);
        }

        #endregion

        #region Constructor

        public SavedMovieDetailViewModel(Movie movie)
        {
            Title = "Movie Details";
            MovieDetail = movie;
            ToolBarItemIcon = "ic_bookmark_24dp.png";
            Task.Run(async () => await LoadDataAsync());
            SetIconToolBar();
        }

        #endregion

        #region Methods


        private void SetIconToolBar()
        {
            try
            {
                var movieFromDB = Services.DB.ArcflixDBContext.MovieDataBase.GetItems()
                    .FirstOrDefault(x => x.IDApi == MovieDetail.Id);
                ToolBarItemIcon = movieFromDB == null ? "ic_bookmark_24dp.png" : "ic_bookmark_white_24dp.png";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                MovieDetail = await MovieDataStore.GetItemAsync(MovieDetail.Id, true);
                if (MovieDetail.Genres != null)
                {
                    var builder = new StringBuilder();
                    var genderList = MovieDetail.Genres.Select(x => x.Name).ToArray();
                    var length = genderList.Length;
                    for (var i = 0; i < length; i++)
                    {
                        if (i == length - 1)
                            builder.Append(genderList[i] + ".");
                        else
                            builder.Append(genderList[i] + ", ");

                    }
                    Genders = builder.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion
    }
}