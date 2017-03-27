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
            List<ObjetoVenta> data = new List<ObjetoVenta>();
            data.Add(new ObjetoVenta("Cocina", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Refrigerador", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Moto", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Auto", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Plancha", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Sarten", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Ropa usada", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Billetera", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));
            data.Add(new ObjetoVenta("Balon", 25000, "https://ayudawp.com/wp-content/uploads/2013/10/miniatura-wordpress.jpg"));

            return View(data);
        }
    }
}