using Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api
{
    public static class MongoDBManager
    {
        static readonly MongoClient Client = new MongoClient("mongodb+srv://FranciscoJMorales:ProyectoED2@projectediicluster.uuw4c.mongodb.net/ChatDB?retryWrites=true&w=majority");

        public static void AddUser(User user)
        {
            var database = Client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Users");
            var document = new BsonDocument
            {
                { user.ID, BsonValue.Create(user) }
            };
            collection.InsertOne(document);
        }

        public static List<User> Users()
        {
            var database = Client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Users");
            var documents = collection.Find(new BsonDocument()).ToList();
            var list = new List<User>();
            foreach (var item in documents)
                list.Add(JsonSerializer.Deserialize<User>(item.ToJson(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            return list;
        }

        public static void AddMessage(Message message)
        {
            var database = Client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Messages");
            var document = new BsonDocument
            {
                { Guid.NewGuid().ToString(), BsonValue.Create(message) }
            };
            collection.InsertOne(document);
        }

        public static List<Message> Messages()
        {
            var database = Client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Messages");
            var documents = collection.Find(new BsonDocument()).ToList();
            var list = new List<Message>();
            foreach (var item in documents)
                list.Add(JsonSerializer.Deserialize<Message>(item.ToJson(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            return list;
        }

        public static List<Message> Chat(string user1, string user2)
        {
            var list = Messages();
            var chat = new List<Message>();
            foreach (var item in list)
            {
                if (item.IsFromChat(user1, user2))
                    chat.Add(item);
            }
            return chat;
        }
    }
}
