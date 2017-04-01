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



        // Metodos
        public ActionResult Index(FormCollection form){
            if (Session["user"] == null){
                return RedirectToAction("Index", "Login");
            }
            // creo lista de objetos a la venta y la envio a la vista


            // prueba 
            ViewBag.pets = tienda.articulosTienda(); 

            return View(tienda.articulosTienda());
        }




        public ActionResult VerArticuloEspecifico(FormCollection form)
        {
            var id = form["oculto"]; // obtengo el id del objeto
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


        public ActionResult incluirArticuloAlInventario() { return View(); }

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



        public ActionResult carrito(){
            if (Session["user"] == null){
                return RedirectToAction("Index", "Login");
            }
            // retorna una lista con los objetos del carrito de compras
            ViewBag.precioTotal = carritoCompras.precioTotal();
            return View(carritoCompras.articulosCarrito());
        }


        public ActionResult ArticuloAlCarrito(FormCollection form){
            // agrega un articulo de la tienda al carrito de compras
            string user = Session["user"].ToString();
            int cantidad = Int32.Parse(form["cantidad"]);
            int idArticulo = Int32.Parse(form["articulo"]);
            if (carritoCompras.agregarAlCarrito(user, idArticulo, cantidad))
            {
                return Redirect("../Tienda/Index");
            }
            return View();
        }



        public ActionResult EliminarArticuloCarrito(FormCollection form){
            // Elimina un articulo de la tienda al carrito de compras
            carritoCompras.eliminarDelCarrito(form["idArticulo"]);
            return Redirect("../Tienda/carrito");
        }



        public ActionResult errorCargarArticuloAlInventario()
        {
            return View();
        }

        public ActionResult errorDatosFaltantesInvetario()
        {
            return View();
        }

        public ActionResult anadirArticuloAlInventario(FormCollection form){
            var nombre = form["nombre"];
            var precio = form["precio"];
            var cantidad = form["cantidad"];
            var img = form["img"];
            ViewBag.nombre = nombre;
            ViewBag.precio = precio;
            ViewBag.cantidad = cantidad;
 
            if ((nombre == "") || (precio == "") || (cantidad == "")){
                return Redirect("../Tienda/errorDatosFaltantesInvetario");
            }
 
            if (tienda.insertarEnInventario(nombre, Int32.Parse(precio), Int32.Parse(cantidad), img)){
                // se creo el objeto
                return View();
            }
            return Redirect("../Tienda/errorCargarArticuloAlInventario");
        }
    }
}