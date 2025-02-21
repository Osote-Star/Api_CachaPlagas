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
    public class UsuariosServices : IUsuarioService
    {
        private MongoClient _client;
        private IMongoDatabase? _database;

        public UsuariosServices(MongoClient client) => _client = client;
        private void Conectar()
        {
            _database = _client.GetDatabase("CachaPlagas");
        }
        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            Conectar();
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }

        public async Task<UsuariosModel> ejemplo() 
        {
            ObtenerColeccion("usuarios");
            return null;
        }

        public Task<UsuariosModel> RecuperarContrasena(string Contrasena)
        {
            throw new NotImplementedException();
        }
    }
}
