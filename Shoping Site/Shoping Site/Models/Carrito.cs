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
                conexionRedis connect = new conexionRedis();
                connect.insertarEnCarrito(user, idArticulo, cantidad);
                return true;
            }
            catch (Exception){
                return false;
            }    
        }

        public bool limpiarCarrito(string user)
        {
            try
            {
                conexionRedis connect = new conexionRedis();
                //Falta agregar procedimiento MySQL para reducir cantidad de inventario
                connect.limpiarCarrito(user);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool eliminarDelCarrito(string user, int idArticulo, int cantidad)
        {
            try
            {
                conexionRedis connect = new conexionRedis();

                connect.eliminarEnCarrito(user, idArticulo);
                return true;
            }catch
            {
                return false;
            }
        }

        public List<Articulo> obtenerCarrito(string user)
        {
            try
            {
                conexionRedis connect = new conexionRedis();
                carrito = connect.obtenerArticulos(user);
                return carrito;
            }
            catch
            {
                return null;
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
                precio += item.Precio; 
            }
            return precio;
        }


        public List<Articulo> articulosCarrito(){
            // retorna todo el carrito 

            return carrito;
        }


        public void eliminarDelCarrito(string id){
            // debo agregar al carrito el objeto con ese id de la tienda
            int idProducto = Int32.Parse(id);
            Articulo obj = null;
            foreach (var item in carrito){
                if (item.ID == idProducto){
                    obj = item;
                    break;
                }
            }
            // Cuando encuentra el articulo lo elimina del carrito
            carrito.Remove(obj);
        }


        
    }
}