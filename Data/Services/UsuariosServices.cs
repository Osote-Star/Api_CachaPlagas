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

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }

        public async Task<List<UsuarioDto>> ConsultarUsuarios()
        {
            var coleccion = ObtenerColeccion("usuarios");

            if (coleccion == null)
            {
                Console.WriteLine("No se pudo obtener la colección 'usuarios'.");
                return new List<UsuarioDto>();
            }

            Console.WriteLine("Realizando la consulta a la colección...");

            // Consultar todos los documentos
            var documentos = await coleccion.Find(new BsonDocument()).ToListAsync();

            Console.WriteLine($"Documentos encontrados: {documentos.Count}");

            if (documentos.Count == 0)
            {
                Console.WriteLine("No se encontraron usuarios en la colección.");
                return new List<UsuarioDto>();
            }

            // Mapear los documentos al DTO
            var usuariosDto = documentos.ConvertAll(doc => new UsuarioDto
            {
                IDUsuario = doc.Contains("IDUsuario") ? doc["IDUsuario"].ToInt32() : 0,
                Email = doc.Contains("Email") ? doc["Email"].AsString : "Sin Email",
                Rol = doc.Contains("Rol") ? doc["Rol"].AsString : "Sin Rol"
            });

            // Mostrar los resultados en consola (opcional)
            usuariosDto.ForEach(u => Console.WriteLine($"ID: {u.IDUsuario}, Email: {u.Email}, Rol: {u.Rol}"));

            return usuariosDto;
        }




    }
}
