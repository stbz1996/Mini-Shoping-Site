using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Tienda
    {
        public void insertarEnInventario(string id, string nombre, string precio, string cantidad, string img){
            conexionMySQL mysql = new conexionMySQL();
            Parametros[] datos = new Parametros[5];
            datos[0] = new Parametros("id", id);
            datos[1] = new Parametros("nombre", nombre);
            datos[2] = new Parametros("precio", precio);
            datos[3] = new Parametros("detalle", "detalle");
            datos[4] = new Parametros("pcantidad", cantidad);
            // falta mandar la img a mongo 
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
        public void eliminarEnInventario(string id){
            conexionMySQL mysql = new conexionMySQL();
            Parametros[] datos = new Parametros[1];
            datos[0] = new Parametros("id", id);
            // falta mandar la img a mongo 
            mysql.eliminarArticulo(datos);
        }



        public bool eliminarArticuloDelInventario(string id){
            return true;
        }

        public Articulo consultarArticulo(string id){
            try
            {
                conexionMySQL mysql = new conexionMySQL();
                Parametros[] datos = new Parametros[1];
                datos[0] = new Parametros("id", id);
                return mysql.consultarArticulo(datos);
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
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosTienda();

            // falta agregar datos a los objetos de la lista, la imagen desde mongo
            foreach (var item in objetosTienda)
            {
                item.Ima = "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg";
            }
            return objetosTienda;
        }
    }
}