using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shoping_Site.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrearCuenta()
        {
            return View();
        }

        public ActionResult Tienda()
        {
            return View();
        }

        public ActionResult VerificaLogin(FormCollection form)
        {
            /* Aqui es donde va la capa logica para vrificarlo con la BD*/
            /* y devolver la vista para el caso */
            var user = form["txtuser"];
            var contrasena = form["txtcont"];

            if ((contrasena == "") || (user == ""))
            {
                return Redirect("../Login/ErrorLogin");
            }
            else
            {
                ViewBag.User = user;
                ViewBag.Contrasena = contrasena;
                return Redirect("../Login/LoginCorrecto");
            }
        }

        public ActionResult ErrorLogin()
        {
            return View();
        }
        public ActionResult LoginCorrecto()
        {
            return View();
        }
    }
}