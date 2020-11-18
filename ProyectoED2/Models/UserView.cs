using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserView
    {
        public string Nombre { get; set; }

        public UserView(User user)
        {
            Nombre = user.Name;
        }
    }
}
