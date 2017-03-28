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
            if (Session["user"] == null)
            {
                return View();
            }
            return RedirectToAction("VerificaLogin", "Login");
        }


        public ActionResult cerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult CrearCuenta()
        {
            return View();
        }





        public ActionResult establecerCuenta(FormCollection form)
        {
            var nombre = form["nombre"];
            var usuario = form["txtuser"];
            var cont = form["cont"];
            var confirmarCont = form["rcont"];

            if ((nombre=="")|| (usuario == "")|| (cont == "")|| (confirmarCont == ""))
            {
                return RedirectToAction("errorDatos", "Login");
            }

            if (cont != confirmarCont){
                return RedirectToAction("errorContrasenas", "Login");
            }

            // aqui debo conectarme con el modelo para mandar a crear la cuanta
            bool estado =  true; // esta variable guarda el resultado para saber si la cuenta se creo o no
            if (estado == false){
                return RedirectToAction("cuentaNoCreada", "Login");
            }
            ViewBag.nombre = nombre;
            ViewBag.usuario = usuario;
            ViewBag.cont = cont;
            return View();
        }

        public ActionResult cuentaNoCreada()
        {
            return View();
        }
        public ActionResult errorContrasenas()
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
            if (Session["user"] == null){
                var user = form["txtuser"];
                var contrasena = form["txtcont"];
                if ((contrasena == "")) {
                    return Redirect("../Login/ErrorConrasenaLogin");
                }
                else if ((user == "")){
                    return Redirect("../Login/ErrorUserLogin");
                }
                else{
                    Session["user"] = user;
                    ViewBag.User = Session["user"];
                    return View();
                }
            }
            else{
                ViewBag.User = Session["user"];
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

        public ActionResult errorDatos()
        {
            return View();
        }

    }
}