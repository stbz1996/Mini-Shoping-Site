using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Controllers.Clases;
using Shoping_Site.Models;

namespace Shoping_Site.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(){
            if (Session["user"] == null) {return View();}
            return RedirectToAction("VerificaLogin", "Login");
        }


        public ActionResult cerrarSesion(){
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult CrearCuenta(){
            return View();
        }


        public ActionResult establecerCuenta(FormCollection form)
        {
            var nombre = form["nombre"];
            var usuario = form["txtuser"];
            var cont = form["cont"];
            var confirmarCont = form["rcont"];

            if ((nombre=="")|| (usuario == "")|| (cont == "")|| (confirmarCont == "")){
                return RedirectToAction("errorDatos", "Login");
            }

            if (cont != confirmarCont){
                return RedirectToAction("errorContrasenas", "Login");
            }

            // aqui debo conectarme con el modelo para mandar a crear la cuanta
            
            // manda a crear la cuenta a la base 
            VerificarLogin log = new VerificarLogin();
            if (log.crearCuenta(nombre, usuario, cont)){
                ViewBag.nombre = nombre;
                ViewBag.usuario = usuario;
                ViewBag.cont = cont;
                return View();
            }
            // si no se creo la cuenta
            return RedirectToAction("cuentaNoCreada", "Login");
            
            
        }


        public ActionResult cuentaNoCreada(){
            return View();
        }


        public ActionResult errorContrasenas(){
            return View();
        }


        public ActionResult Tienda(){
            return View();
        }


        public ActionResult VerificaLogin(FormCollection form){
            /* Aqui es donde va la capa logica para vrificarlo con la BD*/
            /* y devolver la vista para el caso */
            if (Session["user"] == null){
                var user = form["txtuser"];
                var contrasena = form["txtcont"];

                if ((contrasena == ""))  {return Redirect("../Login/ErrorConrasenaLogin");}
                else if ((user == ""))   {return Redirect("../Login/ErrorUserLogin");}
                else{
                    // manda a verificar a la capa de modelos 
                    VerificarLogin log = new VerificarLogin();
                    if (log.verificarUsuario(user, contrasena)){
                        Session["user"] = user;
                        ViewBag.User = Session["user"];
                        return View();
                    }
                    // no es un usuario 
                    return Redirect("../Login/errorUsuarioNoExiste");
                }
            }
            else{
                ViewBag.User = Session["user"];
                return View();
            }
        }


        public ActionResult ErrorConrasenaLogin(){
            return View();
        }


        public ActionResult ErrorUserLogin(){
            ViewBag.msj = "error de usuario";
            return View();
        }


        public ActionResult LoginCorrecto(){
            return View();
        }

        public ActionResult errorDatos() {
            return View();
        }

        public ActionResult errorUsuarioNoExiste(){
            return View();
        }

    }
}