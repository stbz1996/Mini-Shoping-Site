
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cassandra;

namespace Shoping_Site.Models.Secundarias
{
    public class conexionCassandra
    {
        Cluster cluster;
        ISession session;

        public conexionCassandra()
        {
            cluster = Cluster.Builder().AddContactPoint("localhost").Build();
            session = cluster.Connect("proyecto");
        }


        public void agregarComentario(string pUsername, int pIdProducto, string pComentario)
        {
            string query = "INSERT INTO evaluacion(username, idProducto, comentario) values ('" +
                pUsername + "'," + pIdProducto + ", '" + pComentario + "')";
            session.Execute(query);
        }

        
        public List<comentarios> obtenerComentarios(int pIdProducto)
        { 
            List<comentarios> comment = new List<comentarios>();
            RowSet rows = session.Execute("select * from evaluacion where idProducto = " + pIdProducto + " ALLOW FILTERING");
            foreach (Row row in rows){
                comment.Add(new comentarios { user = row[0].ToString() , coment = row[2].ToString() });
            }
            return comment;
        }


        public bool consultaComentario(string pUsername, int pIdProducto)
        {
            RowSet rows = session.Execute("SELECT * FROM evaluacion WHERE idProducto = " + pIdProducto + " ALLOW FILTERING");
            int cantidad = rows.Count();
            if (cantidad == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public void borrarComentarios(int pIdProducto)
        {
            session.Execute("DELETE FROM evaluacion WHERE idProducto = " + pIdProducto + "ALLOW FILTERING");
        }


        // Agregar Calificacion
        public bool agregarCalificacion(string pUsername, int pIdProducto, int calificacion)
        {
            try
            {
                string query = "UPDATE evaluacion SET calificacion = " + calificacion + " WHERE username = '" + pUsername +
                "' AND idProducto = " + pIdProducto;
                session.Execute(query);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        // Obtener calificacion general de cada usuario.

        public int obtenerCalificacion(string pUsername, int pIdProducto)
        {
            try
            {
                string query = "select calificacion from evaluacion where username = '" + pUsername + "' AND idProducto = " + pIdProducto + " ";
                RowSet rows = session.Execute(query);
                int calificacion = -1;
                if (rows.Columns.Length > 0)
                {
                    foreach (Row row in rows)
                    {
                        calificacion = Int32.Parse(row[0].ToString()); // retorna la calificación obtenida previamente.
                    }
                }

                return calificacion;
            }
            catch (Exception e)
            {
                return -1; // Si el valor retornado es null, atrapa la excepción y retorna 0.
            }
        }


        // Obtener promedio de las calificaciones totales de cada producto.

        public double obtenerPromedio(int pIdProducto)
        {
            double promedio = 0.0;
            int omitidos = 0;
            int totalValores = 0;
            RowSet rows = session.Execute("select calificacion from evaluacion where idProducto = " + pIdProducto + " ALLOW FILTERING");
            foreach (Row row in rows)
            {
                totalValores += 1;
                try
                {
                    promedio += Int32.Parse(row[0].ToString());
                }
                catch (Exception e)
                {
                    omitidos += 1;
                }

            }
            promedio = promedio / (totalValores - omitidos);
            return promedio;
        }
    }
}





