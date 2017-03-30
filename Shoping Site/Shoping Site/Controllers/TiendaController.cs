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
            return View(tienda.articulosTienda());
        }

        public ActionResult verDeInventario(){
            return View(tienda.articulosTienda());
        }

        public ActionResult confirmarVerDeInventario()
        {
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

        public ActionResult carrito(){
            if (Session["user"] == null){
                return RedirectToAction("Index", "Login");
            }
            // retorna una lista con los objetos del carrito de compras
            return View(carritoCompras.articulosCarrito());
        }


        public ActionResult ArticuloAlCarrito(FormCollection form){
            // agrega un articulo de la tienda al carrito de compras
            carritoCompras.agregarAlCarrito(form["articulo"]);
            return Redirect("../Tienda/Index");
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