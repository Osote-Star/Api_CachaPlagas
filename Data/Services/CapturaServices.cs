using Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class CapturaServices : ICapturaService
    {
        private MongoClient _client;
        private IMongoDatabase? _database;

        public CapturaServices(MongoClient client) => _client = client;

        public void Conectar()
        {
            _database = _client.GetDatabase("CachaPlagas");
        }

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            Conectar();
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
    }
}
