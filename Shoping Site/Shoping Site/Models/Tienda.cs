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
            // Retorna todos los objetos disponibles en la tienda

            // debe retornar todos los articulos de la tienda
            // debe pedir esos objetos a las bases

            // para probar
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda.Add(new Articulo("1", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("2", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("3", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("4", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("5", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("6", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("7", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("8", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("9", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("10", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("11", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("12", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            objetosTienda.Add(new Articulo("13", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            return objetosTienda;
        }

    }
}