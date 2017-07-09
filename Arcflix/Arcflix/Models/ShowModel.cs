using System;
using System.Collections.Generic;
using Arcflix.Helpers;
using SQLite.Net.Attributes;

namespace Arcflix.Models
{
    public class ShowModel:ObservableObject,IModel
    {
        private bool _isAdded;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int IDApi { get; set; }

        public int VoteCount { get; set; }
        public decimal VoteAverage { get; set; }
        public decimal Popularity { get; set; }
        public int SeasonCount { get; set; }
        public string Status { get; set; }
        public int EpisodeCount { get; set; }
        public string HomePage { get; set; }
        public DateTime? LastAirDate { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public string Backdrop { get; set; }
        public string Poster { get; set; }
        public string Overview { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public bool InProduction { get; set; }
        public bool IsAdded
        {
            get => _isAdded;
            set => SetProperty(ref _isAdded, value);
        }

        public static ShowModel ShowApiToShowModel(System.Net.TMDb.Show show)
        {
            var showModel = new ShowModel
            {
                IDApi = show.Id,
                Name = show.Name,
                Backdrop = show.Backdrop,
                SeasonCount = show.SeasonCount,
                HomePage = show.HomePage,
                ID = 0,
                EpisodeCount = show.EpisodeCount,
                LastAirDate = show.LastAirDate,
                Overview = show.Overview,
                Popularity = show.Popularity,
                Poster = show.Poster,
                FirstAirDate = show.FirstAirDate,
                Status = show.Status,
                OriginalName = show.OriginalName,
                VoteAverage = show.VoteAverage,
                VoteCount = show.VoteCount,
                InProduction = show.InProduction
            };

            return showModel;
        }
        public static IEnumerable<ShowModel> MovieListApiToMovieModelList(IEnumerable<System.Net.TMDb.Show> shows)
        {
            var showsResult = new List<ShowModel>();
            foreach (var show in shows)
            {
                var showModel = ShowApiToShowModel(show);
                showsResult.Add(showModel);
            }
            return showsResult;
        }
    }
}