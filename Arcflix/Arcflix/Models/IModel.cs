using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Arcflix.Models
{
    public interface IModel
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
    }
}