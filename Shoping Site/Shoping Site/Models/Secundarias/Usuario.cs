using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Usuario
    {
        private string name;
        private string username;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
    }
}