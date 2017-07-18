using System.Net.TMDb;
using Arcflix.Models;

namespace Arcflix.Services.DB
{
    public class ArcflixDBContext
    {
        #region User

        private static DatabaseSQLite<User> _userDataBase;
        public static DatabaseSQLite<User> UserDataBase => _userDataBase ?? (_userDataBase = new DatabaseSQLite<User>());

        #endregion

        #region Movie

        private static DatabaseSQLite<MovieModel> _movieDataBase;
        public static DatabaseSQLite<MovieModel> MovieDataBase => _movieDataBase ?? (_movieDataBase = new DatabaseSQLite<MovieModel>());

        #endregion

        #region Show

        private static DatabaseSQLite<ShowModel> _showDataBase;
        public static DatabaseSQLite<ShowModel> ShowDataBase => _showDataBase ?? (_showDataBase = new DatabaseSQLite<ShowModel>());

        #endregion
    }
}