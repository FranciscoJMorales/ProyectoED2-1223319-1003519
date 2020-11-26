using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using Processors;

namespace Models
{
    public class Message : IComparable
    {
        [BsonId]
        public string ID { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }

        public Message()
        {

        }

        public Message(User sender, User receiver, string m)
        {
            ID = Guid.NewGuid().ToString();
            Sender = sender.Name;
            SenderID = sender.ID;
            Receiver = receiver.Name;
            ReceiverID = receiver.ID;
            Content = SDES.CipherMessage(m, sender.Key, receiver.Key);
            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        public bool IsFromChat(string id1, string id2)
        {
            if (SenderID == id1 && ReceiverID == id2)
                return true;
            else
                return SenderID == id2 && ReceiverID == id1;
        }

        public void Decipher(int key)
        {
            Content = SDES.Decipher(Content, key);
        }

        public int CompareTo(object obj)
        {
            try
            {
                return Time.CompareTo(((Message)obj).Time);
            }
            catch
            {
                return 1;
            }
        }
    }
}
