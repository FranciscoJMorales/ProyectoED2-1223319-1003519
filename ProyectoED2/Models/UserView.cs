using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserView
    {
        public string ID { get; set; }
        public string Nombre { get; set; }

        public UserView(User user)
        {
            ID = user.ID;
            Nombre = user.Name;
        }
    }
}
