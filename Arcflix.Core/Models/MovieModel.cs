using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Arcflix.Helpers;
using SQLite.Net.Attributes;

namespace Arcflix.Models
{
    public class MovieModel : ObservableObject, IModel
    {
        private bool _isAdded;

        [PrimaryKey, AutoIncrement]

        public int ID { get; set; }

        public int IDApi { get; set; }
        public int VoteCount { get; set; }
        public decimal VoteAverage { get; set;}
        public decimal Popularity { get; set;}
        public int? Runtime { get; set;}
        public long Revenue { get; set;}
        public string Status { get; set;}
        public DateTime? ReleaseDate { get; set;}
        public string Imdb { get; set;}
        public string HomePage { get; set;}
        public int Budget { get; set;}
        public bool Adult { get; set;}
        public string Backdrop { get; set;}
        public string Poster { get; set;}
        public string Overview { get; set;}
        public string TagLine { get; set;}
        public string OriginalTitle { get; set;}
        public string Title { get; set;}
        public string PosterURL => $@"https://image.tmdb.org/t/p/w185_and_h278_bestv2{Poster}";
        public bool IsAdded
        {
            get => _isAdded;
            set => SetProperty(ref _isAdded, value);
        }

        public static MovieModel MovieApiToMovieModel(System.Net.TMDb.Movie movie)
        {
           var movieModel = new MovieModel
            {
                IDApi = movie.Id,
                Title = movie.Title,
                Adult = movie.Adult,
                Backdrop = movie.Backdrop,
                Budget = movie.Budget,
                HomePage = movie.HomePage,
                ID = 0,
                Imdb = movie.Imdb,
                OriginalTitle = movie.OriginalTitle,
                Overview = movie.Overview,
                Popularity = movie.Popularity,
                Poster = movie.Poster,
                ReleaseDate = movie.ReleaseDate,
                Revenue = movie.Revenue,
                Runtime = movie.Runtime,
                Status = movie.Status,
                TagLine = movie.TagLine,
                VoteAverage = movie.VoteAverage,
                VoteCount = movie.VoteCount,
            };

            return movieModel;
        }

        public static IEnumerable<MovieModel> MovieListApiToMovieModelList(IEnumerable<System.Net.TMDb.Movie> movies)
        {
            var moviesResult = new List<MovieModel>();
            foreach (var movie in movies)
            {
                var movieModel = MovieApiToMovieModel(movie);
                moviesResult.Add(movieModel);
            }
            return moviesResult;
        }

        
    }
}