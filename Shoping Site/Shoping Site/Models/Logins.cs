using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Logins
    {
        private Usuario usuario;
        private conexionNeo4j conexionNeo = new conexionNeo4j();
        private conexionMySQL conexionmysql = new conexionMySQL();
        private Parametros[] datos;

        public Boolean verificarUsuario(string user, string contrasena)
        {
            // conecta con base y retorna true si es un usuario o false si no es un usuario
            if (conexionmysql.abrirConexion()){
                datos = new Parametros[2];
                datos[0] = new Parametros("pUsername", user);
                datos[1] = new Parametros("pPassword", contrasena);
                if (conexionmysql.verificarUsuario(datos) > 0) return true;
                else return false;

            }
            else return false;
        }

        public Boolean crearCuenta(string nombre, string user, string contrasena){
            // conecta con base y retorna true si un usuario se crea o false si no
            try{
                conexionmysql.abrirConexion();
                if (verificarUsuario(user, contrasena)){return false;}
                else{
                    usuario = new Usuario { Name = nombre, Username = user };
                    //conexionNeo.crearUsuario(pNombre, pUsername, pContrasena);
                    datos = new Parametros[3];
                    datos[0] = new Parametros("pUsername", user);
                    datos[1] = new Parametros("pName", nombre);
                    datos[2] = new Parametros("pPassword", contrasena);

                    string username = conexionmysql.insertarUsuario(datos);

                    try {conexionNeo.crearUsuario(user, nombre);}
                    catch (Exception){
                        // elimine el mysql
                        conexionmysql.eliminarUsuario(username);
                        throw;
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Boolean verificarAdmin(string user, string contrasena){
            // conecta con base y retorna true si es un admin o false si no es un admin
            if (conexionmysql.abrirConexion())
            {
                datos = new Parametros[2];
                datos[0] = new Parametros("pUsername", user);
                datos[1] = new Parametros("pPassword", contrasena);

                if (conexionmysql.verificarAdmin(datos) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean crearCuentaAdministrador(string nombre, string user, string password){
            try {
                conexionmysql.abrirConexion();
                if(verificarAdmin(user, password)) {
                    return false;
                }
                else{
                    datos = new Parametros[3];
                    datos[0] = new Parametros("pUsername", user);
                    datos[1] = new Parametros("pName", nombre);
                    datos[2] = new Parametros("pPassword", password);
                    conexionmysql.insertarUsuario(datos);
                    return true;
                }
            }
            catch {
                return false;
            }
        }

        public List<Articulo> mostrarRecomendaciones(string user){
            Tienda tienda = new Tienda();
            // debe retornar cosas que compro el amigo seleccionado
            return tienda.verArticulosTienda();
        }

    }
}