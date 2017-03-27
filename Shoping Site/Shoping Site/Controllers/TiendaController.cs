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
        public ActionResult Index()
        {
            
            // creo lista de objetos a la venta
            List<ObjetoVenta> dataTienda = new List<ObjetoVenta>();
            AlmacenArticulos.llenar();
            dataTienda = AlmacenArticulos.todosObjetosTienda();

            // envia la tienda y el carrito

            return View(dataTienda);
        }

        public ActionResult ArticuloAlCarrito(FormCollection form){
            // agrega un articulo de la tienda al carrito de compras
            var articulo = form["articulo"];// aqui está el ID del articulo
            CarritoCompras.agregarAlCarrito(articulo);
            return Redirect("../Tienda/Index");
        }

        public ActionResult carrito()
        {
            // establece lo que hay en el carrito de compras
            List<ObjetoVenta> dataCarrito = new List<ObjetoVenta>();
            //CarritoCompras.llenar();
            dataCarrito = CarritoCompras.todosObjetosCarrito();
            return View(dataCarrito);
        }
    }
}