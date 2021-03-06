﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Models;
using Shoping_Site.Models.Secundarias;
using System.IO;

namespace Shoping_Site.Controllers 
{
    public class TiendaController : Controller
    {   ////////////////////////////////
        /// atributos de uso general ///
        ////////////////////////////////
        Tienda tienda = new Tienda();
        Carrito carritoCompras = new Carrito();
        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////



        //////////////////////////
        /// Seccion del Tienda ///
        //////////////////////////
        public ActionResult Index(FormCollection form){
            if (Session["user"] == null){return RedirectToAction("Index", "Login");}
            try{
                ViewBag.artic = tienda.verArticulosTienda();
                return View();
            }
            catch (Exception){
                Session["msj"] = "Disculpenos, estamos teniendo problemas para ejecutar su petición, vuelva mas tarde por favor.";
                return Redirect("../Tienda/errorAdmin");
            } 
        }

        public ActionResult incluirArticuloAlInventario() { return View(); }

        public ActionResult anadirArticuloAlInventario(FormCollection form, HttpPostedFileBase imgUpdate){
            var nombre = form["nombre"];
            var precio = form["precio"];
            var cantidad = form["cantidad"];
            var img = form["img"];
            byte[] imageData = null;
            ViewBag.nombre = nombre;
            ViewBag.precio = precio;
            ViewBag.cantidad = cantidad;
            if ((nombre == "") || (precio == "") || (cantidad == "") || (imgUpdate == null)){
                Session["msj"] = "Complete todos los datos por favor";
                return Redirect("../Tienda/errorDatosFaltantesInvetario");
            }
            if (imgUpdate != null && imgUpdate.ContentLength > 0){
                using (var binaryReader = new BinaryReader(imgUpdate.InputStream)){
                    imageData = binaryReader.ReadBytes(imgUpdate.ContentLength);
                }
            }
            try{
                tienda.insertarEnInventario(nombre, precio, cantidad, imageData);
                return View();
            }
            catch (Exception){
                Session["msj"] = "No se logró ingresar el producto al inventario, intentelo mas tarde";
                return Redirect("../Tienda/errorDatosFaltantesInvetario");
            }
        }

        public ActionResult editarInventario()
        {
            if (Session["user"] == null){return RedirectToAction("Index", "Login");}
            try{
                ViewBag.artic = tienda.articulosTienda();
                ViewBag.user = Session["user"];
                return View();
            }
            catch (Exception){
                Session["msj"] = "Disculpenos, estamos teniendo problemas para ejecutar su petición, vuelva mas tarde por favor.";
                return Redirect("../Tienda/errorAdmin");
            }
        }

        public ActionResult editarArticuloInventario(FormCollection form){
            try{
                string id = form["articulo"];
                string nombre = form["nombre"];
                string precio = form["precio"];
                string cantidad = form["cantidad"];
                tienda.actualizarEnInventario(id, nombre, precio, cantidad, "");
                return Redirect("../Tienda/editarInventario");
            }
            catch (Exception){
                Session["msj"] = "Error, no hemos logrado actualizar la información";
                return Redirect("../Tienda/errorAdmin");
            }
            
        }
        //////////////////////////
        //////////////////////////
        //////////////////////////



        /////////////////
        /// Articulos ///
        /////////////////
        public ActionResult msjAtencion()
        {
            return View();
        }
        
        public ActionResult atraparComentario(FormCollection form){
            var comentario = form["comentario"];
            int calificacion = Int32.Parse(form["calificacion"]);
            int idproducto = Int32.Parse(Session["articuloActual"].ToString());
            var user = Session["user"].ToString();
            
            try {
                if (comentario != ""){
                    tienda.insertarComentario(user, idproducto, comentario);
                }
                if ((calificacion != 0) && (tienda.yaCompro(user, idproducto))){
                    try{tienda.calificarArticulo(user, idproducto, calificacion);}
                    catch (Exception){}
                }
               return Redirect("../Tienda/msjAtencion");
            }
            catch (Exception){
                Session["msj"] = "No se logró ingresar el comentario, intentelo mas tarde";
                return Redirect("../Tienda/errorAdmin");
            }
        }

        public ActionResult VerArticuloEspecifico(FormCollection form){
            if (Session["user"] == null){return RedirectToAction("Index", "Login");}
            var id = form["articulo"];
            Session["articuloActual"] = id;
            Articulo art = tienda.consultarArticulo(id);
            ViewBag.nombre = art.Nombre;
            ViewBag.precio = art.Precio;
            ViewBag.cant = art.CantidadMaxima;
            ViewBag.ima = art.Ima;
            ViewBag.calificacion = art.Puntaje;
            List<comentarios> comentarios;
            try{comentarios = tienda.obtenerComentarios(id);}
            catch (Exception){comentarios = new List<Models.Secundarias.comentarios>();}
            ViewBag.comentarios = comentarios;

            // busco recomendaciones
            try{ViewBag.recomendaciones = tienda.obtenerRecomendaciones();}
            catch (Exception){ViewBag.recomendaciones = new List<Articulo>();}
            return View();
        }

        public ActionResult confirmarVerDeInventario(){
            return View(tienda.articulosTienda());
        }
        /////////////////
        /////////////////
        /////////////////



        //////////////////////////////////////
        /// Seccion del carrito de compras ///
        /// //////////////////////////////////
        public ActionResult carrito(){
            // muestra el carrito 
            try{
                if (Session["user"] == null){
                    return RedirectToAction("Index", "Login");
                }
                string user = Session["user"].ToString();
                List<Articulo> lista = carritoCompras.obtenerCarrito(user);
                ViewBag.precioTotal = carritoCompras.precioTotal();
                return View(lista);
            }
            catch (Exception){return Redirect("../Tienda/errorCarrito");}    
        }

        public ActionResult hacerPago() {
            string user = Session["user"].ToString();
            if (tienda.procesarCompra(user)){
                ViewBag.msj = "El pago se realizó Correctamente, su orden será enviada instantaneamente";
            }
            else
            {
                ViewBag.msj = "El pago NO se realizó Correctamente";
            }
            return View();
        }

        public ActionResult ArticuloAlCarrito(FormCollection form){
            // agrega un articulo de la tienda al carrito de 
            string user = Session["user"].ToString();
            int cantidad = Int32.Parse(form["cantidad"]);
            int idArticulo = Int32.Parse(form["articulo"]);
            if (carritoCompras.agregarAlCarrito(user, idArticulo, cantidad)){
                return Redirect("../Tienda/Index");
            }
            // error general
            return Redirect("../Tienda/errorCarrito");
        }

        public ActionResult EliminarArticuloCarrito(FormCollection form){
            // Elimina del carrito de compras
            try
            {
                int idArticulo = Int32.Parse(form["idArticulo"]);
                string user = Session["user"].ToString();
                int cantidadEliminados = Int32.Parse(form["cantidad"]);
                carritoCompras.eliminarDelCarrito(user, idArticulo, cantidadEliminados);
                return Redirect("../Tienda/carrito");
            }
            catch (Exception){
                return Redirect("../Tienda/errorCarrito");
            }
        }
        //////////////////////////////////////
        //////////////////////////////////////
        //////////////////////////////////////



        ///////////////////////////
        ///  Seccion de errores ///
        ///////////////////////////
        public ActionResult errorDatosFaltantesInvetario(){
            ViewBag.msj = Session["msj"].ToString();
            return View();
        }

        public ActionResult errorAdmin(){
            ViewBag.msj = Session["msj"];
            return View();
        }
        ///////////////////////////
        ///////////////////////////
        ///////////////////////////
    }
}