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
        private MongoClient _client;
        public MongoConfiguration(MongoClient client) => _client = client;

        public MongoClient GetClient() => _client;
        //Lo que se ocupa para hacer la conexion epicamente
    }  
}
