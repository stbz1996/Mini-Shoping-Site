using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using ServiceStack.Text;


namespace Shoping_Site.Models.Secundarias
{

    public class articulo{
        public string id { get; set; }
        public int idArticulo { get; set; }
        public int cantidad { get; set; }
    }


    public class conexionRedis{

        RedisClient redis = new RedisClient("localhost", 6379);
        

        public void insertarEnCarrito(string puser, int idProducto, int pcantidad){
            var redisUsers = redis.As<articulo>();
            var nuevo = new articulo { id = puser, idArticulo = idProducto, cantidad = pcantidad };
            redisUsers.Store(nuevo);
        }

        public void limpiarCarrito(string id)
        {
            var articulos = redis.As<articulo>();
            var todosLosArticulos = articulos.GetAll();
        }
    }
}