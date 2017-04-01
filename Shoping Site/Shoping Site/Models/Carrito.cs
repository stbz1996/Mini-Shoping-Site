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
        public static List<Articulo> carrito = new List<Articulo>();


                                            // Metodos
        public bool agregarAlCarrito(string user, int idArticulo, int cantidad){
            // Agrega a REDIS el articulo del carrito
            try{
                conexionRedis conect = new conexionRedis();
                conect.insertarEnCarrito(user, idArticulo, cantidad);
                return true;
            }
            catch (Exception){
                return false;
            }    
        }





























        public bool pagar(string usuario){
            // toma el usuario y el carrito para hacer el pago.

            // limpia el carrito al hacer el pago
            carrito.Clear();
            return true;
        }



        public int precioTotal(){
            // retorna el precio total de la compra            
            int precio = 0;
            foreach (var item in carrito){
                precio += item.precio; 
            }
            return precio;
        }


        public List<Articulo> articulosCarrito(){
            // retorna todo el carrito 
            return carrito;
        }


        public void eliminarDelCarrito(string id){
            // debo agregar al carrito el objeto con ese id de la tienda
            Articulo obj = null;
            foreach (var item in carrito){
                if (item.ID == id){
                    obj = item;
                    break;
                }
            }
            // Cuando encuentra el articulo lo elimina del carrito
            carrito.Remove(obj);
        }


        
    }
}