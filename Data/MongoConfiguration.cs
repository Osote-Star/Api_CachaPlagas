using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data
{
    public class MongoConfiguration
    {
        private string _configuration;
        public MongoConfiguration() { }


        //Lo que se ocupa para hacer la conexion epicamente

        private static IMongoDatabase _database;
        public static void Conectar()
        {
            if (_database == null) 
            {
               string ConexionString = "mongodb+srv://5323100289:cachaplagas@cluster0.csyt4.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
               var client = new MongoClient(ConexionString);
               _database = client.GetDatabase("CachaPlagas");
            }
        }

            // El epico método genérico para obtener cualquier colección :)
            public static IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
    }  
}
