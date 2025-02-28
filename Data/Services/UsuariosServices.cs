using Data.Interfaces;
using DTOs.Usuarios;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
//using BC = BCrypt.Net.BCrypt;


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

        public async Task<UsuariosModel> ejemplo() 
        {
            ObtenerColeccion("usuarios");
            return null;
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


        public async Task<UsuariosModel> AgregarUsuario(CreateUserDto createUserDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");
            //string hashedPassword = BC.HashPassword(createUserDto.Contrasena);


            try
            {
                //Obtener el nuevo IDUsuario 

                int nuevoID = await ObtenerProximoId();

                var NuevoUsuario = new UsuariosModel
                {
                    _Id = ObjectId.GenerateNewId().ToString(),
                    IDUsuario = nuevoID,
                    Email = createUserDto.Email,
                    Contrasena = createUserDto.Contrasena
                };

                var bsonUsuario = NuevoUsuario.ToBsonDocument();
                await collection.InsertOneAsync(bsonUsuario);
                return NuevoUsuario;

            }
            catch(Exception ex)
            {
                return null;
            }

        }

    }
}
