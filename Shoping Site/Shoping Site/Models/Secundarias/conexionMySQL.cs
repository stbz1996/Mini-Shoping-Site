﻿using System;
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

        public string insertarUsuario(Parametros[] datos){
            MySqlCommand cmd = new MySqlCommand("spInsertarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string username =  reader["username"].ToString(); ;
            cerrarConexion();
            return username;
        }

        public void eliminarUsuario(string username)
        {
            MySqlCommand cmd = new MySqlCommand("spEliminarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("user", username);
            MySqlDataReader reader = cmd.ExecuteReader();
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

        public List<Articulo> objetosValidosTienda()
        {
            abrirConexion();
            cmd = new MySqlCommand("call obtenerValidosInventario", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Articulo> objetosTienda = new List<Articulo>();
            while (reader.Read())
            {
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

        public string insertarArticulo(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "insertarProductoAlInventario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++){
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string id= reader["idp"].ToString();
            cerrarConexion();
            return id;
        }

        public void actualizarArticulo(Parametros[] datos){
            abrirConexion();
            cmd.CommandText = "actualizarProductoAlInventario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++){
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

        public List<Articulo> objetosAmigo(string amigo){
            abrirConexion();
            List<Articulo> objetosTienda = new List<Articulo>();
            cmd.CommandText = "spConsultaProductosUsuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("pUsername", amigo);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()){
                int idproducto = Int32.Parse(reader["id"].ToString());
                string nombre = reader["nombre"].ToString();
                int precio = Int32.Parse(reader["precio"].ToString());
                objetosTienda.Add(new Articulo(idproducto, nombre, precio, "", 0, 0));
            }
            cerrarConexion();
            return objetosTienda;
        }

        public void crearAdministrador(Parametros[] datos)
        {
            abrirConexion();
            cmd.CommandText = "spInsertarAdministrador";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            for (int i = 0; i < datos.Length; i++)
            {
                cmd.Parameters.AddWithValue(datos[i].Nombre, datos[i].Valor);
            }
            cmd.ExecuteNonQuery();
            cerrarConexion();
        }

        public bool yaCompro(string user, int idproducto)
        {
            abrirConexion();
            List<Articulo> objetosTienda = new List<Articulo>();
            cmd.CommandText = "yaCompro";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("pUsername", user);
            cmd.Parameters.AddWithValue("pIdProducto", idproducto);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int valor = Int32.Parse(reader["conteo"].ToString());
            cerrarConexion();
            if (valor > 0)
            {
                return true;
            }
            return false;
        }


        public List<int[]> obtenerMejoresProductos(){
            List<int[]> productos = new List<int[]>();
            int[] enteros;
            abrirConexion();
            cmd.CommandText = "obtenerMejoresProductos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()){
                enteros = new int[3];
                enteros[0] = (Int32.Parse(reader["Total"].ToString()));
                enteros[1] = (Int32.Parse(reader["Cantidad"].ToString()));
                enteros[2] = (Int32.Parse(reader["idProducto"].ToString()));
                productos.Add(enteros);
            }
            cerrarConexion();
            return productos;
        }


        public List<Articulo> productosPorOrden(int idorden)
        {
            abrirConexion();
            cmd.CommandText = "productosPorOrden";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            List<Articulo> orden = new List<Articulo>();
                cmd.Parameters.AddWithValue("orden", idorden);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Int32.Parse(reader["id"].ToString());
                    string nombreProducto = reader["nombre"].ToString();
                    int precio = Int32.Parse(reader["precio"].ToString());
                    int cantidad = Int32.Parse(reader["cantidad"].ToString());
                    orden.Add(new Articulo(id, nombreProducto, precio, "", cantidad, 0));
                }
            cerrarConexion();
            return orden;
        }


        public List<List<Articulo>> buscarProductosEnOrden(List<int> list){
            List<List<Articulo>> ordenes = new List<List<Articulo>>();
            foreach (var item in list)
            {
                ordenes.Add(productosPorOrden(item));
            }
            return ordenes;
        }




        public List<List<Articulo>> ordenes(string user)
        {
            abrirConexion();
            cmd.CommandText = "todasLasOrdenesPorUsuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("pUsername", user);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<int> idsOrden = new List<int>();
            while (reader.Read()){
                int idorden = Int32.Parse(reader["idOrden"].ToString());
                idsOrden.Add(idorden);
            }
            cerrarConexion();
            return buscarProductosEnOrden(idsOrden);
        }
    }
}

