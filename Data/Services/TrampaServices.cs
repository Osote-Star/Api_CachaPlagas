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

        #region VincularTrampa
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
        #endregion

        #region MostrarEstadistica
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
        #endregion

        #region CambiarStatusTrampa
        public async Task<TrampaModel> CambiarStatusTrampa(CambiarStatusDto cambiarStatusDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", cambiarStatusDto.IDtrampa);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("EstatusTrampa", cambiarStatusDto.EstatusTrampa)
                    .Set("EstatusPuerta", cambiarStatusDto.EstatusPuerta)
                    .Set("EstatusSensor", cambiarStatusDto.EstatusSensor);
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
        #endregion

        #region CambiarStatusSensor
        public async Task<TrampaModel> CambiarStatusSensor(EstatusSensorDto estatusSensor)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", estatusSensor.IDtrampa);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("EstatusSensor", estatusSensor.estatusSensor);
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
        #endregion

        #region CambiarEstatusPuerta
        public async Task<TrampaModel> CambiarEstatusPuerta(EstatusPuertaDto estatusPuertaDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", estatusPuertaDto.IDtrampa);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("EstatusPuerta", estatusPuertaDto.estatusPuerta);
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
        #endregion

        #region TodasTrampas

        public async Task<List<TrampaModel>> TodasTrampas(int usuarioID)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDUsuario", usuarioID);
                var documentos = await collection.Find(filtro).ToListAsync();
                if (documentos != null && documentos.Count > 0)
                {
                    List<TrampaModel> trampas = new List<TrampaModel>();
                    foreach (var documento in documentos)
                    {
                        trampas.Add(BsonSerializer.Deserialize<TrampaModel>(documento));
                    }
                    return trampas;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region BuscarTrampa

        public async Task<TrampaModel> BuscarTrampa(int trampaID)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", trampaID);
                var documento = await collection.Find(filtro).FirstOrDefaultAsync();
                if (documento != null)
                {
                    return BsonSerializer.Deserialize<TrampaModel>(documento);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region AgregarTrampa
        public async Task<TrampaModel> AgregarTrampa(AgregarTrampaDto agregartrampa)
        {

            var documento = new BsonDocument
            {
                { "IDTrampa", agregartrampa.IDTrampa },
                { "IDUsuario", BsonNull.Value },
                { "Imagen", agregartrampa.Imagen },
                { "Modelo", agregartrampa.Modelo },
                { "Localizacion", BsonNull.Value },
                { "EstatusTrampa", agregartrampa.EstatusTrampa },
                { "EstatusPuerta", agregartrampa.EstatusPuerta },
                { "EstatusSensor", agregartrampa.EstatusSensor }

            };

            await ObtenerColeccion("Trampa").InsertOneAsync(documento);

            return 
            new TrampaModel
            {
                IDTrampa = documento.GetValue("IDTrampa").ToInt32(),
                Imagen = documento.GetValue("Imagen").ToString(),
                Modelo = documento.GetValue("Modelo").ToString(),
                EstatusTrampa = documento.GetValue("EstatusTrampa").ToBoolean(),
                EstatusPuerta = documento.GetValue("EstatusPuerta").ToBoolean(),
                EstatusSensor = documento.GetValue("EstatusSensor").ToBoolean(),
            };
        }
        #endregion

        #region EditarLocalizacion
        public async Task<bool> EditarLocalizacion(EditarLocalizacionDto dto)
        {
            var trampaCollection = ObtenerColeccion("Trampa");

            var filter = Builders<BsonDocument>.Filter.Eq("IDUsuario", Convert.ToInt32(dto.IDUsuario));
            var trampa = await trampaCollection.Find(filter).FirstOrDefaultAsync();

            if (trampa == null)
            {
                Console.WriteLine($"❌ No se encontró la trampa con Id: {dto.IDUsuario}");
                return false;
            }

            Console.WriteLine($"✅ Trampa encontrada en MongoDB: {trampa}");


            var update = Builders<BsonDocument>.Update.Set("Localizacion", dto.Localizacion);

            var result = await trampaCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        #endregion
    }
}
