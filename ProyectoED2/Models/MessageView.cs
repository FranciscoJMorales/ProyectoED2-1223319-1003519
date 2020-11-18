using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class MessageView
    {
        public string Usuario { get; set; }
        public string Fecha { get; set; }
        public string Mensaje { get; set; }

        public MessageView(Message m, string user)
        {
            if (user == m.Sender)
                Usuario = "Tu";
            else
                Usuario = m.Sender;
            Fecha = m.Time.ToString("dd/MM/yyyy HH:mm");
            Mensaje = m.Content;
        }
    }
}
