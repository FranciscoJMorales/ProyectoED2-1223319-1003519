using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Models
{
    public class UserView : IComparable
    {
        public string ID { get; set; }
        public string Nombre { get; set; }

        public UserView()
        {

        }

        public UserView(User user)
        {
            ID = user.ID;
            Nombre = user.Name;
        }

        public int CompareTo(object obj)
        {
            try
            {
                UserView other = (UserView)obj;
                if (Nombre.CompareTo(other.Nombre) == 0)
                    return ID.CompareTo(other.ID);
                else
                    return Nombre.CompareTo(other.Nombre);
            }
            catch
            {
                return 1;
            }
        }
    }
}
