using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Controllers.Clases
{
    public static class CarritoCompras
    {
        // esta clase almacena los articulos que se van agregando al carrito de compras

        // atributos generales
        static List<ObjetoVenta> carrito = new List<ObjetoVenta>();




        public static List<ObjetoVenta> todosObjetosCarrito(){
            // retorna todo el carrito 
            return carrito;
        }

        public static void llenar()
        {
            // este metodo se debe cambiar para que tome los objetos de la BD
            carrito.Add(new ObjetoVenta("id1", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            carrito.Add(new ObjetoVenta("id2", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            carrito.Add(new ObjetoVenta("id3", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            carrito.Add(new ObjetoVenta("id4", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            carrito.Add(new ObjetoVenta("id5", "Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
         }

 
        public static void agregarAlCarrito(string id){
            // debo agregar al carrito el objeto con ese id de la tienda
            ObjetoVenta obj = null;
            foreach (var item in AlmacenArticulos.todosObjetosTienda())
            {
                if (item.ID == id){
                    obj = item;
                    break;
                }
            }
            // Cuando encuentra el articulo lo agrega al carrito
            carrito.Add(obj);
        }
    }
}