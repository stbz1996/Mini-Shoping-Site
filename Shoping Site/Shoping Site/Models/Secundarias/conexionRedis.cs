using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using StackExchange.Redis;
using ServiceStack.Text;


namespace Shoping_Site.Models.Secundarias
{

    /*public class articulo{
        public string id { get; set; }
        public int idArticulo { get; set; }
        public int cantidad { get; set; }
    }*/


    public class conexionRedis{

        //RedisClient redis = new RedisClient("localhost", 6379);

        private ConnectionMultiplexer conn;
        private IDatabase cache;
        private IServer server;

        public conexionRedis()
        {
            conn = RedisConnectorHelper.Connection;
            cache = RedisConnectorHelper.Connection.GetDatabase();
            server = RedisConnectorHelper.server;
        }

        
        public void insertarEnCarrito(string pUser, int pIdProducto, int pCantidad){
            cache.StringSet($"{pUser}:{pIdProducto}", pCantidad);
        }

        public void limpiarCarrito(string pUser)
        {
            foreach (var key in server.Keys(pattern: $"{pUser}*"))
            {
                cache.KeyDelete(key);
            }
            /*server.FlushDatabase(0,);
            var endPoints = conn.GetEndPoints(true);
            foreach(var endPoint in endPoints)
            {
                server.
            }*/
        }

        public void eliminarEnCarrito(string pUser, int pIdProducto)
        {
            foreach (var key in server.Keys(pattern: $"{pUser}:{pIdProducto}*"))
            {
                cache.KeyDelete(key);
            }
        }

        public List<Articulo> obtenerArticulos(string pUser)
        {
            List<Articulo> articulos = new List<Articulo>();
            Articulo articulo;
            foreach(var key in server.Keys(pattern: $"{pUser}:"))
            {
                int cantCaracter = pUser.Length + 1; //Se incrementa en 1 debido a los dos puntos (:)
                int idProducto= Int32.Parse(key.ToString().Remove(0,cantCaracter));
                //Primero se debe consultar en MySQL para el nombre y precio
                //Luego se debe consultar en MongoDB para la imagen
                articulo = new Articulo(idProducto, null, 0, null);
                articulos.Add(articulo);
            }
            return articulos;
        }
        /*public void limpiarCarrito(string id)
        {
            var articulos = redis.As<articulo>();
            var todosLosArticulos = articulos.GetAll();
        }*/
    }
}