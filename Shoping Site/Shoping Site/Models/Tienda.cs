using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Tienda
    {

        public void insertarComentario(string pUsername, int pIdProducto, string pComentario)
        {
            conexionCassandra cass = new conexionCassandra();
            cass.agregarComentario(pUsername, pIdProducto, pComentario);
        }

        public List<comentarios> obtenerComentarios(string id)
        {
            int newId = Int32.Parse(id);
            // abrir conexion con Cassandra
            conexionCassandra cass = new conexionCassandra();
            return cass.obtenerComentarios(newId);
        }

        public void insertarEnInventario(string id, string nombre, string precio, string cantidad, byte[] imagen){
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB conexionMongo = new conexionMongoDB();
            Parametros[] datos = new Parametros[5];
            datos[0] = new Parametros("id", id);
            datos[1] = new Parametros("nombre", nombre);
            datos[2] = new Parametros("precio", precio);
            datos[3] = new Parametros("detalle", "detalle");
            datos[4] = new Parametros("pcantidad", cantidad);
            // falta mandar la img a mongo 
            conexionMongo.insertarBD(id, imagen);
            mysql.insertarArticulo(datos);
        }

        public void actualizarEnInventario(string id, string nombre, string precio, string cantidad, string img)
        {
            conexionMySQL mysql = new conexionMySQL();
            Parametros[] datos = new Parametros[5];
            datos[0] = new Parametros("id", id);
            datos[1] = new Parametros("nombre", nombre);
            datos[2] = new Parametros("precio", precio);
            datos[3] = new Parametros("detalle", "detalle");
            datos[4] = new Parametros("pcantidad", cantidad);
            // falta mandar la img a mongo 
            mysql.actualizarArticulo(datos);
        }
        public bool eliminarEnInventario(string id){
            conexionMongoDB mongo = new conexionMongoDB();
            conexionMySQL mysql = new conexionMySQL();
            Parametros[] datos = new Parametros[1];
            datos[0] = new Parametros("id", id);
            mongo.eliminarBD(id);
            mysql.eliminarArticulo(datos);
            return true;
        }




        public Articulo consultarArticulo(string id){
            try
            {
                conexionMySQL mysql = new conexionMySQL();
                conexionMongoDB conexion = new conexionMongoDB();
                Parametros[] datos = new Parametros[1];
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
            // Retorna todos los objetos disponibles en la tienda ttomados de la BD
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosTienda();

            // falta agregar datos a los objetos de la lista, la imagen desde mongo
            foreach (var item in objetosTienda)
            {
                item.Ima = cnMongo.obtenerBD(item.ID);
            }
            return objetosTienda;
        }
    }
}