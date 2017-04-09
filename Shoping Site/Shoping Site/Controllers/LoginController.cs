using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoping_Site.Models;
using Shoping_Site.Models.Secundarias;

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





        public ActionResult errorDatos() {
            try
            {
                ViewBag.msjError = Session["error"].ToString();
            }
            catch (Exception)
            {
                ViewBag.msjError ="";
            }
            
            return View();
        }



        public ActionResult establecerCuenta(FormCollection form){
            var nombre = form["nombre"];
            var usuario = form["user"];
            var cont = form["cont"];
            var confirmarCont = form["rcont"];

            if ((nombre == "Admin") || (usuario == "Admin")){
                Session["error"] = "El nombre y el usuario no pueden ser Admin";
                return RedirectToAction("errorDatos", "Login");
            }
            if ((nombre == "")){
                Session["error"] = "Debe digitar un nombre";
                return RedirectToAction("errorDatos", "Login");
            }
            if ((usuario == "")){
                Session["error"] = "Debe digitar un nombre de usuario";
                return RedirectToAction("errorDatos", "Login");
            }
            if ((cont == "") || (confirmarCont == "")){
                Session["error"] = "Debe digitar la contraseña";
                return RedirectToAction("errorDatos", "Login");
            }
            if (cont != confirmarCont){
                Session["error"] = "Las contraseñas deben ser iguales";
                return RedirectToAction("errorDatos", "Login");
            }
            // manda a crear la cuenta a la base 
            if (log.crearCuenta(nombre, usuario, cont)){
                ViewBag.nombre = nombre;
                ViewBag.usuario = usuario;
                ViewBag.cont = cont;
                return View();
            }
            // si no se creo la cuenta
            Session["error"] = "Lo sentimos, Su cuenta no pudo ser creada, intentelo de nuevo mas tarde";
            return RedirectToAction("errorDatos", "Login");

              
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

            if (Session["user"] != null)
            {
                ViewBag.UserAdmin = Session["user"];
                return View();
            }

            if (log.verificarAdmin(user, contrasena)){
                ViewBag.UserAdmin = user;
                Session["user"] = user;
                return View();
            }
            return Redirect("../Login/ErrorUserAdmin");
        }


        public ActionResult creaNuevoAdmin(){
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
            if (log.crearCuentaAdministrador(usuario, cont)){
                //ViewBag.nombre = nombre;
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

        public ActionResult ErrorUserLogin() {
            return View();
        }

        public ActionResult LoginCorrecto() { return View(); }

        public ActionResult errorUsuarioNoExiste() { return View(); }

        public ActionResult ModoAdmin() { return View(); }

        public ActionResult ErrorUserAdmin() { return View(); }

        public ActionResult perfil(FormCollection form){
            if (Session["user"] == null) { return RedirectToAction("Index", "Login"); }
            ViewBag.user = Session["user"].ToString();
            string usernameAmigo = form["articulosAmigo"];
            // posee los articulos de el amigo 
            List<Articulo> recomendaciones;
            try{
                recomendaciones = log.mostrarProductosAmigo(usernameAmigo);
                if (recomendaciones == null) { ViewBag.articulosAmigo = new List<Articulo>(); }
                else { ViewBag.articulosAmigo = recomendaciones; }
            }
            catch (Exception) { ViewBag.articulosAmigo = new List<Articulo>(); }
            // posee los amigos
            List<Usuario> amigos;
            try{
                amigos = log.todosLosAmigos(Session["user"].ToString());
                if (amigos == null) { ViewBag.amigos = new List<Usuario>(); }
                else { ViewBag.amigos = amigos; }
            }
            catch (Exception){ViewBag.amigos = new List<Usuario>();}            
            return View();
        }

        public ActionResult buscarUsuario(FormCollection form){
            if (Session["user"] == null) { return RedirectToAction("Index", "Login"); }
            var texto = form["cajaTexto"];
            string usernameAmigo = form["articulosAmigo"];
            Logins log = new Logins();
            @ViewBag.user = Session["user"].ToString();
            // carga la lista de articulos
            List<Articulo> recomendaciones = log.mostrarProductosAmigo(usernameAmigo);
            if (recomendaciones == null) { ViewBag.articulosAmigo = new List<Articulo>(); }
            else { ViewBag.articulosAmigo = recomendaciones;}

            // carga el usuario que se busca
            try{
                Usuario user = log.buscarUsuario(texto.ToString());
                ViewBag.nombre = user.Name;
                ViewBag.username = user.Username;
            }
            catch (Exception) {}

            // carga amigos
            List<Usuario> amigos = log.todosLosAmigos(Session["user"].ToString()); 
            if (amigos == null) { ViewBag.amigos = new List<Usuario>(); }
            else { ViewBag.amigos = amigos; }
            return View();
        }

        public ActionResult seguirUsuario(FormCollection form)
        {
            if (Session["user"] == null) { return RedirectToAction("Index", "Login"); }
            string usernameAmigo = form["oculto"];
            if ((usernameAmigo == Session["user"].ToString()) || (usernameAmigo == null)) { usernameAmigo = ""; }

            try {
                // carga amigos
                Logins log = new Logins();
                List<Usuario> amigos = log.todosLosAmigos(Session["user"].ToString());
                Boolean flagControl = true;
                if (amigos==null){
                    string user = Session["user"].ToString();
                    log.crearRelacion(user, usernameAmigo);
                    return RedirectToAction("buscarUsuario", "Login");
                }

                foreach (var item in amigos){
                    if (item.Username == usernameAmigo){
                        flagControl = false;
                    }
                }
                if (flagControl == true){
                    string user = Session["user"].ToString();
                    log.crearRelacion(user, usernameAmigo);
                }
            }
            catch (Exception){ }
            return RedirectToAction("buscarUsuario", "Login");
        }

    }
}