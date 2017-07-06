namespace Arcflix.Models
{
    public class User:IModel
    {
        public int ID { get; set; }
        public int FacebookID { get; set; }
        public string Name { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}