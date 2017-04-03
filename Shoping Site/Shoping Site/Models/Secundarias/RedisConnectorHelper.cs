using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;

namespace Shoping_Site.Models.Secundarias
{

    public class RedisConnectorHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;

        static RedisConnectorHelper(){
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect("localhost");
            });
        }

        public static ConnectionMultiplexer Connection{
            get { return lazyConnection.Value;}
        }

        public static IServer server{
            get { return Connection.GetServer("localhost", 6379); }
        }

    }
}