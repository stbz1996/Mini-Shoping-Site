using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using Shoping_Site.Models.Secundarias;

namespace Shoping_Site.Models
{
    public class conexionMySQL
    {

        /////////////////
        /// Atributos ///
        /////////////////
        private MySqlConnection conn = new MySqlConnection("server=localhost; userId=root; password=admin; database=proyecto;");
        private MySqlCommand cmd;

        ///////////////
        /// Metodos ///
        ///////////////
        public conexionMySQL() { }

        public bool abrirConexion(){
            try{
                cmd = new MySqlCommand();
                conn.Open();
                return true;
            }
            catch{
                return false;
            } 
        }

        public int consultaNombreUsuarios(Parametros[] datos){
            abrirConexion();
            cmd.CommandText = "spConsultaNombreUsuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue(datos[0].Nombre, datos[0].Valor);
            MySqlDataAdapter data = new MySqlDataAdapter(cmd);
            DataTable tabla = new DataTable();
            data.Fill(tabla);
            return tabla.Rows.Count;
        }

        public int verificarUsuario(Parametros[] datos){
            abrirConexion();
            cmd.CommandText = "spVerificaUsuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataAdapter data = new MySqlDataAdapter(cmd);
            DataTable tabla = new DataTable();
            data.Fill(tabla);
            cerrarConexion();
            return tabla.Rows.Count;
        }

        public int verificarAdmin(Parametros[] datos){
            abrirConexion();
            cmd.CommandText = "spVerificaAdmin";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataAdapter data = new MySqlDataAdapter(cmd);
            DataTable tabla = new DataTable();
            data.Fill(tabla);
            cerrarConexion();
            return tabla.Rows.Count;
        }

        public void insertarUsuario(Parametros[] datos){
            MySqlCommand cmd = new MySqlCommand("spInsertarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            cmd.ExecuteNonQuery();
            cerrarConexion();
        }

        public void insertarAdmin(Parametros[] datos)
        {
            MySqlCommand cmd = new MySqlCommand("spInsertarAdministrador", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            for(int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            cmd.ExecuteNonQuery();
            cerrarConexion();
        }

        public void cerrarConexion(){
            conn.Close();
        }

        public List<Articulo> objetosTienda(){
            abrirConexion();
            cmd = new MySqlCommand("call obtenerInventario", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Articulo> objetosTienda = new List<Articulo>();
            while (reader.Read()){
                int idproducto = Int32.Parse(reader["idProducto"].ToString());
                string nombre = reader["nombreProducto"].ToString();
                int precio = Int32.Parse(reader["precio"].ToString());
                int cantidadTotal = Int32.Parse(reader["cantidad"].ToString());
                objetosTienda.Add(new Articulo(idproducto, nombre, precio, "", 0, cantidadTotal));
            }
            cerrarConexion();
            return objetosTienda;
        }

        public Articulo consultarArticulo(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "consultarArticulo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue(datos[0].Nombre, datos[0].Valor);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string nombre = reader["nombre"].ToString();
            int precio = Int32.Parse(reader["precio"].ToString());
            int cantidadTotal = Int32.Parse(reader["cantidad"].ToString());
            int idproducto  = Int32.Parse(reader["idProducto"].ToString());
            cerrarConexion();
            return new Articulo(idproducto, nombre, precio, "", 0, cantidadTotal);
        }

        public void insertarArticulo(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "insertarProductoAlInventario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++){
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            cerrarConexion();
        }

        public void actualizarArticulo(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "actualizarProductoAlInventario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            cerrarConexion();
        }

        public void eliminarArticulo(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "eliminarDeInventario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            cerrarConexion();
        }

        public void realizarOrden(Parametros[] datos) {
            abrirConexion();
            cmd.CommandText = "insertarEnOrden";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for(int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            cerrarConexion();
        }


        public int crearOrden(string user){
            abrirConexion();
            cmd.CommandText = "insertarOrden";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("pUsername", user);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string valor = reader["valor"].ToString();
            cerrarConexion();
            return Int32.Parse(valor);
        }

    }
}

