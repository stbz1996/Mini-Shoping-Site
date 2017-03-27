using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Controllers.Clases;

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

            if ((contrasena == ""))
            {
                return Redirect("../Login/ErrorConrasenaLogin");
            }
            else if ((user == ""))
            {
                return Redirect("../Login/ErrorUserLogin");
            }
            else
            {
                Session["idUsuario"] = user;
                ViewBag.User = user;
                ViewBag.Contrasena = contrasena;
                return View();
            }
        }

        public ActionResult ErrorConrasenaLogin()
        {
            return View();
        }
        public ActionResult ErrorUserLogin()
        {
            return View();
        }
        public ActionResult LoginCorrecto()
        {
            return View();
        }

    }
}