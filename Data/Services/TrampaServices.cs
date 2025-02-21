using Data.Interfaces;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TrampaServices : ITrampaServices
    {
        private IMongoDatabase? _database;

        public TrampaServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
        public async Task<TrampaModel> VincularTrampa(int trampaID)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            return null;
        }
    }
}
