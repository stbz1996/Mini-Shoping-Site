using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Controllers.Clases;

namespace Shoping_Site.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index(FormCollection form)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            // creo lista de objetos a la venta
            List<ObjetoVenta> dataTienda = new List<ObjetoVenta>();
            AlmacenArticulos.llenar();
            dataTienda = AlmacenArticulos.todosObjetosTienda();
            return View(dataTienda);
        }






        public ActionResult ArticuloAlCarrito(FormCollection form){
            // agrega un articulo de la tienda al carrito de compras
            var articulo = form["articulo"];// aqui está el ID del articulo
            CarritoCompras.agregarAlCarrito(articulo);
            return Redirect("../Tienda/Index");
        }

        public ActionResult EliminarArticuloCarrito(FormCollection form)
        {
            // agrega un articulo de la tienda al carrito de compras
            var articulo = form["idArticulo"];
            CarritoCompras.eliminarDelCarrito(articulo);
            return Redirect("../Tienda/carrito");
        }

        public ActionResult carrito()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            // establece lo que hay en el carrito de compras
            List<ObjetoVenta> dataCarrito = new List<ObjetoVenta>();
            //CarritoCompras.llenar();
            dataCarrito = CarritoCompras.todosObjetosCarrito();
            return View(dataCarrito);
        }
    }
}