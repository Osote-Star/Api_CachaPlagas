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
        public async Task<UsuariosModel> RecuperarContrasena(RecuperarContrasenaDto recuperarContrasenaDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");

            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDUsuario", recuperarContrasenaDto.IDUsuario);
                var actualizacion = Builders<BsonDocument>.Update
                    .Set("Contrasena", recuperarContrasenaDto.Contrasena);

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
    }
}
