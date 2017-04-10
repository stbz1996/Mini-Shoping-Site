using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shoping_Site.Models.Secundarias
{
    public static class Encriptacion
    {
        // http://www.programadordepalo.com/access-encriptar-contrasenas-con-sha-256-utilizando-biblioteca-de-clases-net-con-c/
        // Autor del código: Arkaitz Arteaga. 

        private static string encriptarSha256(string palabraEncriptar)
        {
            // proveedor de servicios de la encriptación
            SHA256CryptoServiceProvider proveedor = new SHA256CryptoServiceProvider();

            // Arreglos de bytes 
            byte[] bytes = Encoding.UTF8.GetBytes(palabraEncriptar);
            byte[] bytesCodificados = proveedor.ComputeHash(bytes); // Calcula el valor hash del objeto.

            StringBuilder palabraEncriptada = new StringBuilder(); // Representa una mutable string caracteres.

            for (int i = 0; i < bytesCodificados.Length; i++)
                palabraEncriptada.Append(bytesCodificados[i].ToString("x2").ToLower()); // encriptó en palabra Encriptada.

            return palabraEncriptada.ToString();
        }

        // Encripta la contraseña.
        public static string encriptar(string contraseña)
        {
            return encriptarSha256(contraseña);
        }
    }
}