using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Models;
using Shoping_Site.Models.Secundarias;

namespace Shoping_Site.Controllers 
{
    public class TiendaController : Controller
    {
        // atributos de uso general 
        Tienda tienda = new Tienda();
        Carrito carritoCompras = new Carrito();


        //////////////////////////
        /// Seccion del Tienda ///
        //////////////////////////
        public ActionResult Index(FormCollection form){
            if (Session["user"] == null){
                return RedirectToAction("Index", "Login");
            }
            // Carga la tienda. 
            try{
                ViewBag.artic = tienda.articulosTienda();
                return View();
            }
            catch (Exception){
                return Redirect("../Tienda/errorCarrito");
            } 
        }

        public ActionResult incluirArticuloAlInventario() { return View(); }

        public ActionResult anadirArticuloAlInventario(FormCollection form)
        {
            var id = form["id"];
            var nombre = form["nombre"];
            var precio = form["precio"];
            var cantidad = form["cantidad"];
            var img = form["img"];
            ViewBag.nombre = nombre;
            ViewBag.precio = precio;
            ViewBag.cantidad = cantidad;

            if ((nombre == "") || (precio == "") || (cantidad == ""))
            {
                return Redirect("../Tienda/errorDatosFaltantesInvetario");
            }

            try{
                tienda.insertarEnInventario(id, nombre, precio, cantidad, img);
                return View();
            }
            catch (Exception){
                return Redirect("../Tienda/errorCargarArticuloAlInventario");
            }
        }

        public ActionResult editarInventario()
        {
            if (Session["user"] == null){
                return RedirectToAction("Index", "Login");
            }
            try
            {
                ViewBag.artic = tienda.articulosTienda();
                ViewBag.user = Session["user"];
                return View();
            }
            catch (Exception)
            {
                return Redirect("../Tienda/errorCarrito");
            }
        }

        public ActionResult editarArticuloInventario(FormCollection form){
            try{
                if (Int32.Parse(form["cantidad"]) != 0){
                    string id = form["articulo"];
                    string nombre = form["nombre"];
                    string precio = form["precio"];
                    string cantidad = form["cantidad"];
                    tienda.actualizarEnInventario(id, nombre, precio, cantidad, "hola");
                }
                else{
                    string id = form["articulo"];
                    tienda.eliminarEnInventario(id);
                }
                return Redirect("../Tienda/editarInventario");
            }
            catch (Exception){
                return Redirect("../Tienda/errorArticulosAdmin");
            }
            
        }

        public ActionResult errorArticulosAdmin()
        {
            return View();
        }
        //////////////////////////
        //////////////////////////
        //////////////////////////



        public ActionResult VerArticuloEspecifico(FormCollection form)
        {
            // obtengo los datos del producto
            var id = form["articulo"];
            Articulo art = tienda.consultarArticulo(id);
            ViewBag.nombre = art.Nombre;
            ViewBag.precio = art.Precio;
            ViewBag.cant = art.CantidadMaxima;
            
            // Aqui debo mandar a traer los comentarios
            List<String> comentarios = new List<String>();
            comentarios.Add("es un buen producto");
            comentarios.Add("me cago en la puta, esto no sirve para nada tio, me cago en la leche con estos estafadores, oyo Jorge jajaja");
            comentarios.Add("jajaja vale picha esta madre ya jajaja");
            ViewBag.comentarios = comentarios;

            // necesito la images
            // necesito la valoracion
            // necesito las recomendaciones 

            //var id = form["oculto"]; // obtengo el id del objeto
            // 
            return View();
        }








        public ActionResult verDeInventario(){
            return View(tienda.articulosTienda());
        }

        public ActionResult confirmarVerDeInventario(){
            return View(tienda.articulosTienda());
        }


        public ActionResult eliminarDeInventario(FormCollection form){
            var id = form["oculto"];
            // envio a que se elimina
            if (tienda.eliminarArticuloDelInventario(id)){
                return RedirectToAction("confirmarVerDeInventario", "Tienda");
            }
            return View(tienda.articulosTienda());
        }


       
        public ActionResult hacerPago() {
            // Conecta con la base para hacer el pago de la compra
            string x = Session["user"].ToString();

            if (carritoCompras.pagar(x)){
                ViewBag.msj = "El pago se realizó Correctamente, su orden será enviada instantaneamente";
            }
            else{
                ViewBag.msj = "El pago NO se realizó Correctamente";
            }
            return View();
        }



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
                ViewBag.precioTotal = carritoCompras.precioTotal(user);
                return View(lista);
            }
            catch (Exception){return Redirect("../Tienda/errorCarrito");}    
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
            catch (Exception)
            {
                return Redirect("../Tienda/errorCarrito");
            }

        }

        public ActionResult errorCarrito()
        {
            return View();
        }
        //////////////////////////////////////
        //////////////////////////////////////
        //////////////////////////////////////


        public ActionResult errorCargarArticuloAlInventario()
        {
            return View();
        }

        public ActionResult errorDatosFaltantesInvetario()
        {
            return View();
        }

       
    }
}