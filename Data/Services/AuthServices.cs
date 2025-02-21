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
    public class AuthServices : IAuthServices
    {
        private IMongoDatabase? _database;

        public AuthServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
        public Task<UsuariosModel> RecuperarContrasena(string Contrasena)
        {
            throw new NotImplementedException();
        }
    }
}
