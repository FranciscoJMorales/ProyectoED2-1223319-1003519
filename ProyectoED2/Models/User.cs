using System;
using MongoDB.Bson.Serialization.Attributes;
using Processors;

namespace Models
{
    public class User : IComparable
    {
        [BsonId]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Key { get; set; }

        public User()
        {

        }

        public User(string name, string password)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            var cesar = new CesarEncryptor();
            Password = cesar.CipherPassword(password);
            Random rng = new Random();
            Key = rng.Next(0, 1021);
        }

        public int CompareTo(object obj)
        {
            try
            {
                User other = (User)obj;
                if (Name.CompareTo(other.Name) == 0)
                    return ID.CompareTo(other.ID);
                else
                    return Name.CompareTo(other.Name);
            }
            catch
            {
                return 1;
            }
        }
    }
}
