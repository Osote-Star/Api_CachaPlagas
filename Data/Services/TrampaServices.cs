using Data.Interfaces;
using DTOs.TrampaDto;
using DTOs.UsuariosDto;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<TrampaModel> VincularTrampa(VincularTrampaDto vincularTrampaDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");

            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", vincularTrampaDto.TrampaID);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("IDUsuario", vincularTrampaDto.UsuarioID);

                var nuevoDocumento = new FindOneAndUpdateOptions<BsonDocument>
                {
                    ReturnDocument = ReturnDocument.After // Retorna el documento actualizado
                };

                var documentoActualizado = await collection.FindOneAndUpdateAsync(filtro, actualizacion, nuevoDocumento);

                if (documentoActualizado != null)
                {
                    return BsonSerializer.Deserialize<TrampaModel>(documentoActualizado);
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TrampaModel> MostrarEstadistica(int TrampaId)
        {
            var collection = ObtenerColeccion("Trampa");
            try
            {
                // Pipeline optimizado
                var pipeline = new[]
                {
                    new BsonDocument("$match", new BsonDocument("IDTrampa", TrampaId)),
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Captura" },
                        { "localField", "IDTrampa" },
                        { "foreignField", "IDTrampa" },
                        { "as", "Capturas" }
                    })
                  };

                // Ejecutar pipeline y obtener el primer documento
                var documentoUnido = await collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

                if (documentoUnido == null)
                    return null;

                // Deserializar directamente a TrampaModel
                var trampa = BsonSerializer.Deserialize<TrampaModel>(documentoUnido);

                return trampa;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
