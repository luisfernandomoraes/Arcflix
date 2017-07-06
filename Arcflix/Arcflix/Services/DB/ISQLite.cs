using SQLite.Net;

namespace Arcflix.Services.DB
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}