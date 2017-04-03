using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using StackExchange.Redis;
using ServiceStack.Text;




namespace Shoping_Site.Models.Secundarias
{
    public class conexionRedis
    {
        private ConnectionMultiplexer conn;
        private IDatabase cache;
        private IServer server;

        public conexionRedis(){
            conn = RedisConnectorHelper.Connection;
            cache = RedisConnectorHelper.Connection.GetDatabase();
            server = RedisConnectorHelper.server;
        }

        ///////////////
        /// Carrito ///
        ///////////////
        public void insertarEnCarrito(string pUser, int pIdProducto, int pCantidad){
            cache.StringSet($"{pUser}:{pIdProducto}", pCantidad);
        }

        public void limpiarCarrito(string pUser){
            foreach (var key in server.Keys(pattern: $"{pUser}*")){
                cache.KeyDelete(key);
            }
        }

        public void eliminarEnCarrito(string pUser, int pIdProducto, int cantidad) {
            // necesito buscar el articulo primero
            if (cantidad > 0){
                insertarEnCarrito(pUser, pIdProducto, cantidad);
            }
            else{
                foreach (var key in server.Keys(pattern: $"{pUser}:{pIdProducto}*")) {
                    cache.KeyDelete(key);
                }
            }
        }

        public List<Articulo> obtenerArticulos(string pUser){
            List<Articulo> articulos = new List<Articulo>();
            foreach (var key in server.Keys(pattern: $"{pUser}*")){
                int cantCaracter = pUser.Length + 1; //Se incrementa en 1 debido a los dos puntos (:)
                int idProducto = Int32.Parse(key.ToString().Remove(0,cantCaracter));
                int cantidad = Int32.Parse(cache.StringGet(key));
                articulos.Add(new Articulo(idProducto, "", 0, "", cantidad, 0));
            }
            return articulos;
        }
        ///////////////
        ///////////////
        ///////////////
    }
}