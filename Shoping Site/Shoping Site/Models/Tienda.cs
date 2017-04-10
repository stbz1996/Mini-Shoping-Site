using Shoping_Site.Models.Secundarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoping_Site.Models
{
    public class Tienda
    {

        Parametros[] datos;

        public void insertarComentario(string pUsername, int pIdProducto, string pComentario)
        {
            conexionCassandra cass = new conexionCassandra();
            cass.agregarComentario(pUsername, pIdProducto, pComentario);
        }

        public List<comentarios> obtenerComentarios(string id){
            int newId = Int32.Parse(id);
            conexionCassandra cass = new conexionCassandra();
            List < comentarios > comentarios = cass.obtenerComentarios(newId);
            List<comentarios> comentFinal = new List<comentarios>();
            foreach (var item in comentarios)
            {
                if ((item.coment != "") || (item.coment != null)){comentFinal.Add(item);}
            }
            return comentFinal;
        }

        public void insertarEnInventario(string nombre, string precio, string cantidad, byte[] imagen){
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB conexionMongo = new conexionMongoDB();
            datos = new Parametros[4];
            datos[0] = new Parametros("nombre", nombre);
            datos[1] = new Parametros("precio", precio);
            datos[2] = new Parametros("detalle", "detalle");
            datos[3] = new Parametros("pcantidad", cantidad);
            string id = mysql.insertarArticulo(datos);
            conexionMongo.insertarBD(id, imagen);
        }

        public void actualizarEnInventario(string id, string nombre, string precio, string cantidad, string img){
            conexionMySQL mysql = new conexionMySQL();
            datos = new Parametros[5];
            datos[0] = new Parametros("id", id);
            datos[1] = new Parametros("nombre", nombre);
            datos[2] = new Parametros("precio", precio);
            datos[3] = new Parametros("detalle", "detalle");
            datos[4] = new Parametros("pcantidad", cantidad);
            // falta mandar la img a mongo 
            mysql.actualizarArticulo(datos);
        }

        public Articulo consultarArticulo(string id){
            try{
                conexionMySQL mysql = new conexionMySQL();
                conexionMongoDB conexion = new conexionMongoDB();
                conexionCassandra cass = new conexionCassandra();
                datos = new Parametros[1];
                datos[0] = new Parametros("id", id);
                Articulo temp = mysql.consultarArticulo(datos);
                temp.Ima = conexion.obtenerBD(Int32.Parse(id));
                try{
                    temp.Puntaje = cass.obtenerPromedio(temp.ID);
                }
                catch (Exception){ temp.Puntaje = 10; }
                
                return temp;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public List<Articulo> articulosTienda(){
            // Retorna todos los objetos disponibles en la tienda
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosTienda();
            foreach (var item in objetosTienda){
                item.Ima = cnMongo.obtenerBD(item.ID);
                string x = item.Nombre;
            }
            return objetosTienda;
        }

        public List<Articulo> verArticulosTienda()
        {
            // Retorna todos los objetos disponibles en la tienda
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosTienda = new List<Articulo>();
            objetosTienda = mysql.objetosValidosTienda();
            foreach (var item in objetosTienda)
            {
                item.Ima = cnMongo.obtenerBD(item.ID);
            }
            return objetosTienda;
        }

        public bool procesarCompra(string pUser) {
            // carrito desde redis
            string user = pUser;
            Carrito car = new Carrito();
            List<Articulo> carrito = car.obtenerCarrito(user);
            int total = car.precioTotal();

            // crear la orden
            conexionMySQL conexionmysql = new conexionMySQL();
            int numOrden = conexionmysql.crearOrden(user);
            
            // llenar la orden 
            Parametros[] datos = new Parametros[3];
            datos[0] = new Parametros("pIdOrden", numOrden.ToString());
            foreach (var item in carrito){
                datos[1] = new Parametros("pIdProducto", item.ID.ToString());
                datos[2] = new Parametros("pCantidad", item.Cantidad.ToString());
                conexionmysql.realizarOrden(datos);
            }
            // limpio el carrito
            car.limpiarCarrito(user);
            return true;
        }

        public List<Articulo> verArticulosAmigo(string amigo){
            // Retorna todos los objetos disponibles en la tienda
            conexionMySQL mysql = new conexionMySQL();
            conexionMongoDB cnMongo = new conexionMongoDB();
            List<Articulo> objetosAmigo = new List<Articulo>();
            objetosAmigo = mysql.objetosAmigo(amigo);
            foreach (var item in objetosAmigo){
                item.Ima = cnMongo.obtenerBD(item.ID);
            }
            return objetosAmigo;
        }

        public bool calificarArticulo( string usuario, int producto, int calificacion){
            conexionCassandra cass = new conexionCassandra();
            return cass.agregarCalificacion(usuario, producto, calificacion);
        }

        public bool yaCompro(string user, int idproducto)
        {
            conexionMySQL conect = new conexionMySQL();
            return conect.yaCompro(user, idproducto);
        }


        public List<Articulo> obtenerRecomendaciones(){
            // variables
            Object[] arreglo = new Object[2];
            List<int[]> productos;
            List<double> promedios = new List<double>();
            List<Articulo> articulos = new List<Articulo>();
            // conexiones
            conexionMySQL cnMySQL = new conexionMySQL();
            conexionCassandra cnCassandra = new conexionCassandra();
            conexionMongoDB cnMongo = new conexionMongoDB();

            productos = cnMySQL.obtenerMejoresProductos();
            int sumTotal = 0;
            int calificacionTotal = 5;
            double calificacionObtenida;
            sumTotal = obtenerSumatoria(productos);
       
            for (int i = 0; i < productos.Count; i++)
            {
                int[] listaTemp = productos.ElementAt(i);
                int cantidadUnProducto = listaTemp[0];
                calificacionObtenida = Double.Parse(cnCassandra.obtenerPromedio(listaTemp[2]).ToString());
                if (Double.IsNaN(calificacionObtenida) == true)
                {
                    calificacionObtenida = 2.5;
                }
                promedios.Add(((calificacionObtenida + calificacionTotal) * sumTotal) / (sumTotal - cantidadUnProducto));
            }

            arreglo[0] = productos;
            arreglo[1] = promedios;
            productos = insertionSort(arreglo); // Se realiza el insertion Sort.
            int x;
            if (productos.Count < 10){ x = 0;}
            else { x = productos.Count - 10;}
            for (int i = x; i < productos.Count; i++){
                int idProducto = productos[i][2];
                string imagen = cnMongo.obtenerBD(idProducto);
                Parametros[] datos = { new Parametros("id", idProducto.ToString()) };
                Articulo articulo = cnMySQL.consultarArticulo(datos);
                articulo.Ima = cnMongo.obtenerBD(idProducto);
                articulos.Add(articulo);
            }
            return articulos;
        }


        // funciones
        private int obtenerSumatoria(List<int[]> productos){
            int sumTotal = 0;
            foreach (int[] listaTemp in productos){
                sumTotal += listaTemp[0];
            }
            return sumTotal;
        }

        private List<int[]> insertionSort(Object[] arreglo){
            List<int[]> productos = (List<int[]>)arreglo[0];
            List<double> promedios = (List<double>)arreglo[1];
            int largo = promedios.Count;
            for (int i = 1; i < largo; i++){
                int j = i;
                while ((j > 0) && (promedios[j] < promedios[j - 1])){
                    int k = j - 1;
                    double tempPromedios = promedios[k];
                    // swap de promedios //
                    promedios[k] = promedios[j];
                    promedios[j] = tempPromedios;
                    // swap de enteros //
                    Swap(productos, j, k);
                    j--;
                }
            }
            return productos;
        }

        private static void Swap<T>(IList<T> list, int indexA, int indexB){
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }


        public List<List<Articulo>> ordenes(string user)
        {
            conexionMySQL con = new conexionMySQL();
            conexionMongoDB mong = new conexionMongoDB();
            List<List<Articulo>> ordenes = con.ordenes(user);
            // faltan imagenes y mas datos
            foreach (var orden in ordenes)
            {
                foreach (var item in orden)
                {
                    item.Ima = mong.obtenerBD(item.ID);
                }
            }
            return ordenes;
        }



    }
}