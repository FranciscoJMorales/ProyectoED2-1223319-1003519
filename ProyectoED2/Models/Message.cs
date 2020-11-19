using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public Message()
        {

        }

        public Message(string user1, string user2, string m)
        {
            Sender = user1;
            Receiver = user2;
            Content = m;
            Time = DateTime.Now;
        }
    }
}
