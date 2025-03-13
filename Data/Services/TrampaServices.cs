﻿using Data.Interfaces;
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

                var opciones = new FindOneAndUpdateOptions<BsonDocument>
                {
                    ReturnDocument = ReturnDocument.After // Retorna el documento actualizado
                };

                var documentoActualizado = await collection.FindOneAndUpdateAsync(filtro, actualizacion, opciones);

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

        public async Task<TrampaModel> AgregarTrampa(AgregarTrampaDto agregartrampa)
        {

            var documento = new BsonDocument
            {
                { "IDTrampa", agregartrampa.IDTrampa },
                { "IDUsuario", BsonNull.Value },
                { "Imagen", agregartrampa.Imagen },
                { "Modelo", agregartrampa.Modelo },
                { "Localizacion", BsonNull.Value },
                { "Estatus", agregartrampa.EstatusTrampa },
                { "Estatus", agregartrampa.EstatusPuerta },
                { "Estatus", agregartrampa.EstatusSensor }

            };

            await ObtenerColeccion("Trampa").InsertOneAsync(documento);

            return new TrampaModel
            {
                IDTrampa = documento.GetValue("IDTrampa").ToInt32(),
                Imagen = documento.GetValue("Imagen").ToString(),
                Modelo = documento.GetValue("Modelo").ToString(),
                EstatusTrampa = documento.GetValue("EstatusTrampa").ToString(),
                EstatusPuerta = documento.GetValue("EstatusPuerta").ToString(),
                EstatusSensor = documento.GetValue("EstatusSensor").ToString(),
            };
        }
    }
}
