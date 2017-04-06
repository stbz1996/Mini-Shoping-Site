﻿using Shoping_Site.Models.Secundarias;
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
        public int total = 0;

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
                conexionMySQL mysql = new conexionMySQL();
                conexionMongoDB cnMongo = new conexionMongoDB();
                carrito = connect.obtenerArticulos(user);
                Articulo temp = null;
                // paso 2: completar el nombre, el precio del articulo, la imagen y el total que existen en el inventario
                Parametros[] datos = new Parametros[1];
                foreach (Articulo item in carrito){
                    datos[0] = new Parametros("id", item.ID.ToString());
                    temp = mysql.consultarArticulo(datos);
                    item.Nombre = temp.Nombre;
                    item.Precio = temp.Precio;
                    item.CantidadMaxima = temp.CantidadMaxima;
                    item.Ima = cnMongo.obtenerBD(item.ID);
                    total = total + (item.Precio * item.Cantidad);
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
            return this.total;
        }


       
        
    }
}