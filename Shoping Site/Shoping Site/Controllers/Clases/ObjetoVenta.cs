using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Controllers.Clases
{
    public class ObjetoVenta
    {
        public string nombre { get; set; }
        public int precio { get; set; }
        public string ima { get; set; }

        public ObjetoVenta(string nombre, int precio, string ima) {
            this.nombre = nombre;
            this.precio = precio;
            this.ima = ima;

        }
    }
}