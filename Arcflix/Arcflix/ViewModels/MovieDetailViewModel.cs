using System;
using System.Diagnostics;
using System.Linq;
using System.Net.TMDb;
using System.Text;
using System.Threading.Tasks;
using Arcflix.Services;

namespace Arcflix.ViewModels
{
    public class MovieDetailViewModel:BaseViewModel
    {
        private string _genders;

        #region Properties

        public Movie MovieDetail { get; private set; }

        public string Genders
        {
            get => _genders;
            set => SetProperty(ref _genders,value);
        }

        #endregion

        #region Constructor

        public MovieDetailViewModel(Movie movie)
        {
            Title = "Movie Details";
            MovieDetail = movie;
            Task.Run(async()=> await LoadDataAsync());
        }

        #endregion

        #region Methods

        public async Task  LoadDataAsync()
        {
            try
            {
                MovieDetail = await MovieDataStore.GetItemAsync(MovieDetail.Id, true);
                if (MovieDetail.Genres != null)
                {
                    var builder = new StringBuilder();
                    var genderList = MovieDetail.Genres.Select(x => x.Name).ToArray();
                    var length = genderList.Length;
                    for (var i = 0;i< length; i++)
                    {
                        if (i == length - 1)
                            builder.Append(genderList[i]);
                        else
                            builder.Append(genderList[i]+", ");

                    }
                    Genders = "Genders: " + builder;
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