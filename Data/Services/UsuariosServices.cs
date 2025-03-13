using Data.Interfaces;
using DTOs.UsuariosDto;
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
        private IMongoDatabase? _database;

        public UsuariosServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion()
        {
            return _database.GetCollection<BsonDocument>("Usuario");
        }

        public async Task<IEnumerable<UsuarioDto>> ConsultarUsuario()
        {
            var filtro = Builders<BsonDocument>.Filter.Empty;
            var documentos = await ObtenerColeccion().Find(filtro).ToListAsync();

            return documentos.Select(doc => new UsuarioDto
            {
                IDUsuario = doc.GetValue("IDUsuario").AsInt32,
                Email = doc.GetValue("Email").AsString,
                Rol = doc.GetValue("Rol").AsString
            });
        }

    }
}
