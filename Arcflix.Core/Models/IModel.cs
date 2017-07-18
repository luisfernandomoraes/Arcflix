using SQLite.Net.Attributes;

namespace Arcflix.Models
{
    public interface IModel
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
    }
}