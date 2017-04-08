using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;

namespace Shoping_Site.Models
{
    public class conexionNeo4j
    {
        private GraphClient conexionNeo;

        public conexionNeo4j()
        {
            conexionNeo = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "admin"); ;
        }


        public void crearUsuario(string username, string name)
        {

            var newUser = new Usuario { Username = username, Name = name };
            conexionNeo.Connect();

            //Se crea un nodo en la base
            conexionNeo.Cypher
                .Create("(user:User {newUser})")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResults();
        }

        //Metodo que permite la creacion de relacion de 2 usuarios
        public void crearRelacion(string pUsername1, string pUsername2)
        {
            //Realizamos union entre dos usuarios (grafo dirigido)
            conexionNeo.Cypher
                .Match("(user1:User)", "(user2:User)")
                .Where((Usuario user1) => user1.Username == pUsername1)
                .AndWhere((Usuario user2) => user2.Username == pUsername2)
                .Create("(user1)-[:FOLLOW_WITH]->(user2)")
                .ExecuteWithoutResults();
        }

        //Metodo que retorna los usuarios que sigue la persona
        public List<Usuario> retornarRelaciones(string pUsername)
        {
            //Obtenemos arreglo de personas que sigue el usuario
            var personasASeguir = conexionNeo.Cypher
                .OptionalMatch("(user:User)-[FOLLOW_WITH]-(friend:User)")
                .Where((Usuario user) => user.Username == pUsername)
                .Return((user, friend) => new
                {
                    Friends = friend.CollectAs<Usuario>()
                })
                .Results;

            int cantidad = personasASeguir.Single().Friends.Count();

            //Verificamos cuantos amigos posee
            if (cantidad != 0)
            {
                //Obtenemos los datos de cada usuario y lo almacenamos en una lista de Usuario
                List<Usuario> usuariosASeguir = new List<Usuario>();
                for(int i = 0; i < cantidad; i++)
                {
                    string username = personasASeguir.Single().Friends.ElementAt(i).Username;
                    string name = personasASeguir.Single().Friends.ElementAt(i).Name;
                    usuariosASeguir.Add(new Usuario { Username = username, Name = name });
                }
                return usuariosASeguir;
            }
            else
            {
                return null;
            }
            
        }
    }
}