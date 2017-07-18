using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using SQLite.Net.Attributes;

namespace Arcflix.Models
{
    public class User : IModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public long FacebookID { get; set; }
        public string Name { get; set; }

        [Ignore]
        public string ProfilePictureURL => $@"http://graph.facebook.com/{FacebookID}/picture?type=large";

        

        public static User FromRawJsonFacebook2Object(string rawResponse)
        {
            try
            {
                var obj = JObject.Parse(rawResponse);
                var id = obj["id"].ToString().Replace("\"", "");
                var fullName = obj["name"]?.ToString().Replace("\"", "");

                var user = new User
                {
                    FacebookID = Convert.ToInt64(id),
                    Name = fullName,
                };

                return user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }
    }
}