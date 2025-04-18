﻿using Data.Interfaces;
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
    public class CapturaServices : ICapturaService 
    {
        private IMongoDatabase _database;

        public CapturaServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }

        //sobrecarga de obtenerColeecion
        public IMongoCollection<TrampaModel> ObtenerColeccion<TrampaModel>(string nombreColeccion)
        {
            return _database.GetCollection<TrampaModel>(nombreColeccion);
        }

        //Metodo para ObtenerProximoId
        public async Task<int> ObtenerProximoIdcaptura()
        {
            IMongoCollection<BsonDocument> collection1 = ObtenerColeccion("Trampa");
            IMongoCollection<CapturaModel> collection = ObtenerColeccion<CapturaModel>("Captura");

            // Obtener todos los usuarios y extraer el _Id máximo
            var captura = await collection.Find(_ => true)
                                           .Sort(Builders<CapturaModel>.Sort.Descending(u => u.IDCaptura))
                                           .Limit(1)
                                           .ToListAsync();

            if (captura.Any())
            {
                return captura.First().IDCaptura + 1; // Retornar el siguiente ID disponible
            }

            return 1; // Si no hay usuarios, comenzar desde 1
        }


        public async Task<CapturaModel> AgregarCaptura(int TrampaId)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Captura");
            try
            {
                int nuevoID = await ObtenerProximoIdcaptura();

                // Obtener hora UTC y convertir a la zona horaria de Hermosillo
                TimeZoneInfo hermosilloTimeZone = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time"); // Windows
                DateTime horaHermosillo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, hermosilloTimeZone);

                var nuevacaptura = new CapturaModel
                {
                    _Id = ObjectId.GenerateNewId().ToString(),
                    IDCaptura = nuevoID,
                    IDTrampa = TrampaId,
                    FechaCaptura = horaHermosillo, // Usamos la hora convertida
                };

                var bsonservice = nuevacaptura.ToBsonDocument();
                await collection.InsertOneAsync(bsonservice);

                return nuevacaptura;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
