﻿using Models;
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

        public static bool AddUser(User user)
        {
            if (FindUser(user.Name, user.Password) == null)
            {
                var database = Client.GetDatabase("ChatDB");
                var collection = database.GetCollection<BsonDocument>("Users");
                var document = new BsonDocument
                {
                    { user.ID, BsonValue.Create(user) }
                };
                collection.InsertOne(document);
                return true;
            }
            else
                return false;
        }

        public static List<User> Users()
        {
            var database = Client.GetDatabase("ChatDB");
            var collection = database.GetCollection<BsonDocument>("Users");
            var documents = collection.Find(new BsonDocument()).ToList();
            var list = new List<User>();
            foreach (var item in documents)
                list.Add(JsonSerializer.Deserialize<User>(item.ToJson(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            list.Sort();
            return list;
        }

        public static User FindUser(string name, string password)
        {
            var list = Users();
            foreach (var item in list)
            {
                if (item.Name == name && item.Password == password)
                    return item;
            }
            return null;
        }

        public static User FindUser(string id)
        {
            var list = Users();
            foreach (var item in list)
            {
                if (item.ID == id)
                    return item;
            }
            return null;
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

        public static List<Message> Chat(string id1, string id2)
        {
            var list = Messages();
            var chat = new List<Message>();
            foreach (var item in list)
            {
                if (item.IsFromChat(id1, id2))
                    chat.Add(item);
            }
            var user1 = FindUser(id1);
            var user2 = FindUser(id2);
            foreach (var item in chat)
                item.Decipher(user1.Key, user2.Key);
            return chat;
        }

        public static List<MessageView> ChatView(string id1, string id2)
        {
            var list = Chat(id1, id2);
            var chat = new List<MessageView>();
            foreach (var item in list)
                chat.Add(new MessageView(item, id1));
            chat.Sort();
            return chat;
        }

        public static List<MessageView> Search(string id1, string id2, string text)
        {
            var results = new List<MessageView>();
            if (text != null)
            {
                if (text.Length > 0)
                {
                    var list = ChatView(id1, id2);
                    foreach (var item in list)
                    {
                        if (item.Mensaje.Contains(text))
                            results.Add(item);
                    }
                }
            }
            results.Sort();
            return results;
        }
    }
}
