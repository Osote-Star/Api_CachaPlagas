using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Data
{
    public class MongoConfiguration
    {
        private string _configuration;
        public MongoConfiguration() { }
        public static void Conexion()
        {
            string conexion = "mongodb+srv://5323100289:cachaplagas@cluster0.csyt4.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var client = new MongoClient(conexion);
            var database = client.GetDatabase("CachaPlagas");




        }
       

         
    }
}
