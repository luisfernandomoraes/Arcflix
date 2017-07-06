namespace Arcflix.Services.DB
{
    public class ArcflixDBContext
    {
        private static DatabaseSQLite<Models.User> _userDataBase;
        public static DatabaseSQLite<Models.User> UserDataBase => _userDataBase ?? (_userDataBase = new DatabaseSQLite<Models.User>());
    }
}