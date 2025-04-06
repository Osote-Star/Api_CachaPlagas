using Data.Interfaces;
using DTOs.TrampaDto;
using DTOs.Usuarios;
using DTOs.UsuariosDto;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;



namespace Data.Services
{
    public class UsuariosServices : IUsuarioService
    {
        private IMongoDatabase? _database;

        public UsuariosServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }

        //sobrecarga de obtenerColeecion
        public IMongoCollection<UsuariosModel> ObtenerColeccion<UsuariosModel>(string nombreColeccion)
        {
            return _database.GetCollection<UsuariosModel>(nombreColeccion);
        }

        public async Task<UsuariosModel> CambiarContrasena(CambiarContrasenaDto cambiarContrasenaDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");
            string hashedPassword = BC.EnhancedHashPassword(cambiarContrasenaDto.Contrasena);
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("Email", cambiarContrasenaDto.Email);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("Contrasena", hashedPassword);

                var nuevoDocumento = new FindOneAndUpdateOptions<BsonDocument>
                {
                    ReturnDocument = ReturnDocument.After // Retorna el documento actualizado
                };

                var documentoActualizado = await collection.FindOneAndUpdateAsync(filtro, actualizacion, nuevoDocumento);

                if (documentoActualizado != null)
                {
                    return BsonSerializer.Deserialize<UsuariosModel>(documentoActualizado);
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Metodo para ObtenerProximoId
        public async Task<int> ObtenerProximoId()
        {
            IMongoCollection<BsonDocument> collection1 = ObtenerColeccion("Usuario");
            IMongoCollection<UsuariosModel> collection = ObtenerColeccion<UsuariosModel>("Usuario");

            // Obtener todos los usuarios y extraer el _Id máximo
            var usuarios = await collection.Find(_ => true)
                                           .Sort(Builders<UsuariosModel>.Sort.Descending(u => u.IDUsuario))
                                           .Limit(1)
                                           .ToListAsync();

            if (usuarios.Any())
            {
                return usuarios.First().IDUsuario + 1; // Retornar el siguiente ID disponible
            }

            return 1; // Si no hay usuarios, comenzar desde 1
        }


        public async Task<UsuariosModel?> AgregarUsuario(CreateUserDto createUserDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");
            //string hashedPassword = BC.HashPassword(createUserDto.Contrasena);

            string hashedPassword = BC.EnhancedHashPassword(createUserDto.Contrasena);
            try
            {
                //Obtener el nuevo IDUsuario 

                int nuevoID = await ObtenerProximoId();

                var NuevoUsuario = new UsuariosModel
                {
                    _Id = ObjectId.GenerateNewId().ToString(),
                    IDUsuario = nuevoID,
                    Email = createUserDto.Email,
                    Contrasena = hashedPassword
                };

                var bsonUsuario = NuevoUsuario.ToBsonDocument();
                await collection.InsertOneAsync(bsonUsuario);
                return NuevoUsuario;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #region ConsultarUsuario
        public async Task<IEnumerable<UsuarioDto>> ConsultarUsuario()
        {
            var filtro = Builders<BsonDocument>.Filter.Empty;
            var documentos = await ObtenerColeccion("Usuario").Find(filtro).ToListAsync();

            return documentos.Select(doc => new UsuarioDto
            {
                IDUsuario = doc.GetValue("IDUsuario").AsInt32,
                Email = doc.GetValue("Email").AsString,
                Rol = doc.GetValue("Rol").AsString
            });
        }
        #endregion


    }
}