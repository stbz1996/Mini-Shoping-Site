using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Tienda
    {

        Parametros[] datos;

        public void insertarComentario(string pUsername, int pIdProducto, string pComentario)
        {
            conexionCassandra cass = new conexionCassandra();
            cass.agregarComentario(pUsername, pIdProducto, pComentario);
        }

        public List<comentarios> obtenerComentarios(string id){
            int newId = Int32.Parse(id);
            conexionCassandra cass = new conexionCassandra();
            return cass.obtenerComentarios(newId);
        }

        public void insertarEnInventario(string nombre, string precio, string cantidad, byte[] imagen){
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB conexionMongo = new conexionMongoDB();
            datos = new Parametros[4];
            datos[0] = new Parametros("nombre", nombre);
            datos[1] = new Parametros("precio", precio);
            datos[2] = new Parametros("detalle", "detalle");
            datos[3] = new Parametros("pcantidad", cantidad);
            string id = mysql.insertarArticulo(datos);
            conexionMongo.insertarBD(id, imagen);
        }

        public void actualizarEnInventario(string id, string nombre, string precio, string cantidad, string img){
            conexionMySQL mysql = new conexionMySQL();
            datos = new Parametros[5];
            datos[0] = new Parametros("id", id);
            datos[1] = new Parametros("nombre", nombre);
            datos[2] = new Parametros("precio", precio);
            datos[3] = new Parametros("detalle", "detalle");
            datos[4] = new Parametros("pcantidad", cantidad);
            // falta mandar la img a mongo 
            mysql.actualizarArticulo(datos);
        }

        public Articulo consultarArticulo(string id){
            try{
                conexionMySQL mysql = new conexionMySQL();
                conexionMongoDB conexion = new conexionMongoDB();
                datos = new Parametros[1];
                datos[0] = new Parametros("id", id);
                Articulo temp = mysql.consultarArticulo(datos);
                temp.Ima = conexion.obtenerBD(Int32.Parse(id));
                return temp;
            }
            catch (Exception)
            {
                // mandar un error
                return null;
            }
            
        }

        public List<Articulo> articulosTienda(){
            // Retorna todos los objetos disponibles en la tienda
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosTienda();
            foreach (var item in objetosTienda){
                item.Ima = cnMongo.obtenerBD(item.ID);
            }
            return objetosTienda;
        }

        public List<Articulo> verArticulosTienda()
        {
            // Retorna todos los objetos disponibles en la tienda
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosValidosTienda();
            foreach (var item in objetosTienda)
            {
                item.Ima = cnMongo.obtenerBD(item.ID);
            }
            return objetosTienda;
        }

        public bool procesarCompra(string pUser) {
            // carrito desde redis
            string user = pUser;
            Carrito car = new Carrito();
            List<Articulo> carrito = car.obtenerCarrito(user);
            int total = car.precioTotal();

            // crear la orden
            conexionMySQL conexionmysql = new conexionMySQL();
            int numOrden = conexionmysql.crearOrden(user);
            
            // llenar la orden 
            Parametros[] datos = new Parametros[3];
            datos[0] = new Parametros("pIdOrden", numOrden.ToString());
            foreach (var item in carrito){
                datos[1] = new Parametros("pIdProducto", item.ID.ToString());
                datos[2] = new Parametros("pCantidad", item.Cantidad.ToString());
                conexionmysql.realizarOrden(datos);
            }
            // limpio el carrito
            car.limpiarCarrito(user);
            return true;
        }

    }
}