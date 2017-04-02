using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models.Secundarias
{
    public class Articulo
    {
        private int idArticulo;
        private string nombre;
        private int precio;
        private string ima;

        public int ID
        {
            get { return idArticulo; }
            set { idArticulo = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        public string Ima
        {
            get { return ima; }
            set { ima = value; }
        }
        

        public Articulo(int id, string nombre, int precio, string ima)
        {
            this.nombre = nombre;
            this.precio = precio;
            this.ima = ima;
            this.idArticulo = id;
        }

    }
}