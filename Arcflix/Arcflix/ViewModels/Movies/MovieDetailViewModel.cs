using System;
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

namespace Arcflix.ViewModels.Movies
{
    public class MovieDetailViewModel : BaseViewModel
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
                var isMovieSaved = IsMovieSaved(MovieDetail.IDApi);
                if (isMovieSaved)
                {
                    ToolBarItemIcon = "ic_bookmark_24dp.png";
                    var movie = Services.DB.ArcflixDBContext.MovieDataBase.GetItems()
                        .FirstOrDefault(x => x.IDApi == MovieDetail.IDApi);
                    Arcflix.Services.DB.ArcflixDBContext.MovieDataBase.DeleteItem(movie.ID);
                    MovieDetail.IsAdded = false;
                    App.ShowToast(ToastNotificationType.Success, "Arcflix", "movie removed!", 3);

                }
                else
                {
                    ToolBarItemIcon = "ic_bookmark_white_24dp.png";
                    var movieModel = MovieDetail;
                    Arcflix.Services.DB.ArcflixDBContext.MovieDataBase.SaveItem(movieModel);
                    MovieDetail.IsAdded = true;
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

        public MovieModel MovieDetail { get; private set; }

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

        public MovieDetailViewModel(MovieModel movie)
        {
            Title = "Movie Details";
            MovieDetail = movie;
            ToolBarItemIcon = "ic_bookmark_24dp.png";
            Task.Run(async () => await LoadDataAsync());
            SetIconToolBar();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns true if a movie is saved in local database.
        /// </summary>
        /// <param name="movieId">Id of movie</param>
        /// <returns></returns>
        public static bool IsMovieSaved(int movieId)
        {
            var movieFromDB = Services.DB.ArcflixDBContext.MovieDataBase.GetItems()
                .FirstOrDefault(x => x.IDApi == movieId);
            return movieFromDB != null;
        }
        private void SetIconToolBar()
        {
            try
            {
                bool isAdded = IsMovieSaved(MovieDetail.IDApi);
                ToolBarItemIcon = !isAdded ? "ic_bookmark_24dp.png" : "ic_bookmark_white_24dp.png";
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
                var movieFromApi = await MovieDataStore.GetItemAsync(MovieDetail.IDApi, true);
                if (movieFromApi.Genres != null)
                {
                    var builder = new StringBuilder();
                    var genderList = movieFromApi.Genres.Select(x => x.Name).ToArray();
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