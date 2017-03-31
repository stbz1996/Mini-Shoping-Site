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
        public static List<Articulo> carrito = new List<Articulo>();




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


        public void agregarAlCarrito(string id){
            // debo agregar al carrito el objeto con ese id de la tienda
            Articulo obj = null;
            Tienda tienda = new Tienda();
            foreach (var item in tienda.articulosTienda()){
                if (item.ID == id){
                    obj = item;
                    break;
                }
            }
            // Cuando encuentra el articulo lo agrega al carrito
            carrito.Add(obj);
        }
    }
}