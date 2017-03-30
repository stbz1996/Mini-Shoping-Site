using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Models;

namespace Shoping_Site.Controllers
{
    public class LoginController : Controller
    {
        // Atributos generales
        Logins log = new Logins();


        public ActionResult Index(){
            if (Session["user"] == null) { return View(); }
            return RedirectToAction("VerificaLogin", "Login");
        }


        public ActionResult cerrarSesion(){
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult CrearCuenta(){
            return View();
        }

        

       
        public ActionResult establecerCuenta(FormCollection form){
            var nombre = form["nombre"];
            var usuario = form["txtuser"];
            var cont = form["cont"];
            var confirmarCont = form["rcont"];

            if ((nombre=="")|| (usuario == "")|| (cont == "")|| (confirmarCont == "")){
                return RedirectToAction("errorDatos", "Login");}
            if (cont != confirmarCont){
                return RedirectToAction("errorContrasenas", "Login");}

            // manda a crear la cuenta a la base 
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
            if (Session["user"] == null){
                var user = form["txtuser"];
                var contrasena = form["txtcont"];

                // habilita el modo de administrador
                if ((user == "Admin") && (contrasena == "Admin")){return Redirect("../Login/ModoAdmin"); }

                // habilita el modo de usuario
                if ((contrasena == ""))  {return Redirect("../Login/ErrorConrasenaLogin");}
                else if ((user == ""))   {return Redirect("../Login/ErrorUserLogin");}
                else{
                    // manda a verificar a la capa de modelos
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


        public ActionResult VerificAdmin(FormCollection form){
            var user = form["txtuser"];
            var contrasena = form["txtcont"];
            if (log.verificarAdmin(user, contrasena)){
                ViewBag.UserAdmin = user;
                Session["user"] = user;
                return View();
            }
            return Redirect("../Login/ErrorUserAdmin");
        }


        public ActionResult creaNuevoAdmin()
        {
            return View();
        }

        public ActionResult estableceNuevoAdmin(FormCollection form){
            var nombre = form["nombre"];
            var usuario = form["txtuser"];
            var cont = form["cont"];
            var confirmarCont = form["rcont"];
            // debe retornar las vistas especiales
            if ((nombre == "") || (usuario == "") || (cont == "") || (confirmarCont == "")){
                return RedirectToAction("ErrorDatosAdmin", "Login");
            }
            if (cont != confirmarCont){
                return RedirectToAction("ErrorConrasenaAdmin", "Login");
            }
            // manda a crear la cuenta a la base 
            if (log.crearCuentaAdministrador(nombre, usuario, cont)){
                ViewBag.nombre = nombre;
                ViewBag.usuario = usuario;
                ViewBag.cont = cont;
                return View();
            }
            // si no se creo la cuenta
            return RedirectToAction("cuentaNoCreadaAdmin", "Login");
        }

        public ActionResult cuentaNoCreadaAdmin() { return View(); }

        public ActionResult ErrorConrasenaLogin() { return View(); }

        public ActionResult ErrorConrasenaAdmin() { return View(); }

        public ActionResult ErrorDatosAdmin() { return View(); }

        public ActionResult ErrorUserLogin() { return View(); }

        public ActionResult LoginCorrecto() { return View(); }

        public ActionResult errorDatos() { return View(); }

        public ActionResult errorUsuarioNoExiste() { return View(); }

        public ActionResult ModoAdmin() { return View(); }

        public ActionResult ErrorUserAdmin() { return View(); }

    }
}