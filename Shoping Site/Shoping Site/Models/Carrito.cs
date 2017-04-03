using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Carrito
    {
        // atributo general para almacenar los articulos del carrito durante su uso
        public List<Articulo> carrito = new List<Articulo>();
        public int total;

                                            // Metodos
        public bool agregarAlCarrito(string user, int idArticulo, int cantidad){
            try{
                conexionRedis connect = new conexionRedis();
                connect.insertarEnCarrito(user, idArticulo, cantidad);
                return true;
            }
            catch (Exception){
                return false;
            }    
        }

        public bool limpiarCarrito(string user){
            try{
                conexionRedis connect = new conexionRedis();
                connect.limpiarCarrito(user);
                return true;
            }catch(Exception){ 
                return false;
            }
        }

        public bool eliminarDelCarrito(string user, int idArticulo, int cantidad){
            try{
                conexionRedis connect = new conexionRedis();
                connect.eliminarEnCarrito(user, idArticulo, cantidad); // faltan añadir datos al los articulo 
                return true;
            }catch{
                return false;
            }
        }






        /// falta terminarlo 
        public List<Articulo> obtenerCarrito(string user){
            try{
                // paso 1: obtener los objetos del carrito en redis
                conexionRedis connect = new conexionRedis();
                carrito = connect.obtenerArticulos(user);

                // paso 2: completar el nombre, el precio del articulo y la imagen
                foreach(Articulo item in carrito){
                    item.Nombre = "xxx";
                    item.Precio = 10000;
                    item.Ima = "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg";
                }
                return carrito;
            }
            catch{
                return null;
            }
        }



        public bool pagar(string usuario){
            // toma el usuario y el carrito para hacer el pago.

            // limpia el carrito al hacer el pago
            carrito.Clear();
            return true;
        }



        public int precioTotal(string user){
            // retorna el precio total de la compra            
            total = 10000;
            return this.total;
        }


       
        
    }
}