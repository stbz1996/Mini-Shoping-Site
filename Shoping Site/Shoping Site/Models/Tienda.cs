using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Tienda
    {
        public bool insertarEnInventario(string nombre, int precio, int cantidad, string img){
            // inserta objetos en el inventario
            // se conecta en la base para crear un objeto en la tienda
            return true;
        }

        public bool eliminarArticuloDelInventario(string id){
            return true;
        }



        public List<Articulo> articulosTienda(){
            // Retorna todos los objetos disponibles en la tienda ttomados de la BD
            conexionMySQL mysql = new conexionMySQL();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosTienda();

            // falta agregar datos a los objetos de la lista, la imagen desde mongo
            foreach (var item in objetosTienda)
            {
                item.Ima = "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg";
            }
            return objetosTienda;
        }
    }
}