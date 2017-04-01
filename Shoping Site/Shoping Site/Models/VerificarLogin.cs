using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;

namespace Shoping_Site.Models
{
    public class VerificarLogin
    {


        public Boolean verificarUsuario(string user, string contrasena){
            // conecta con base y retorna true si es un usuario o false si no es un usuario
            return true;
        }


        public Boolean crearCuenta(string nombre, string user, string contrasena){
            // conecta con base y retorna true si es un usuario se crea o false si no
            try
            {
                Usuario usuario = new Usuario();
                usuario.agregarUsuario(nombre, user, contrasena);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public Boolean verificarAdmin(string user, string contrasena)
        {
            // conecta con base y retorna true si es un admin o false si no es un admin
            return true;
        }

    }
}