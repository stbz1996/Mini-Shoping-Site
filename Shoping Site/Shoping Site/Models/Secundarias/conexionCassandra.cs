
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
            string query = "INSERT INTO comentarios(username, idProducto, comentario) values ('" +
                pUsername + "'," + pIdProducto + ", '" + pComentario + "')";
            session.Execute(query);
        }

        public void actualizarComentario(string pUsername, int pIdProducto, string pComentario)
        {
            string query = "UPDATE comentarios SET comentario = '" + pComentario + "' WHERE idProducto = " +
                pIdProducto + "AND username = '" + pUsername + "'";
            session.Execute(query);
        }

        public List<comentarios> obtenerComentarios(int pIdProducto)
        { 
            List<comentarios> comment = new List<comentarios>();
            RowSet rows = session.Execute("select * from comentarios where idProducto = " + pIdProducto + " ALLOW FILTERING");
            foreach (Row row in rows){
                comment.Add(new comentarios { user = row[0].ToString() , coment = row[2].ToString() });
            }
            return comment;
        }


        public bool consultaComentario(string pUsername, int pIdProducto)
        {
            RowSet rows = session.Execute("SELECT * FROM comentarios WHERE idProducto = " + pIdProducto + " ALLOW FILTERING");
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
            session.Execute("DELETE FROM comentarios WHERE idProducto = " + pIdProducto + "ALLOW FILTERING");
        }
    }
}





