using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;

namespace Shoping_Site.Models
{
    public class conexionNeo4j
    {
        private GraphClient conexionNeo = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "admin");

        public conexionNeo4j()
        {
            conexionNeo = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "admin"); ;
        }


        public void crearUsuario(string username, string name)
        {
            var newUser = new Usuario { Username = username, Name = name };
            conexionNeo.Connect();
            conexionNeo.Cypher
                .Create("(user:User {newUser})")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResults();
        }
    }
}