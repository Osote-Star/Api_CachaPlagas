﻿using Data.Interfaces;
using DTOs.TrampaDto;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.SignalR;
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

        private readonly IHubContext<TrampaHub> _hubContext;

        public TrampaServices(MongoConfiguration client, IHubContext<TrampaHub> hubContext)
        {
            _database = client.GetClient().GetDatabase("CachaPlagas");
            _hubContext = hubContext;
        }

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
        public IMongoCollection<T> ObtenerColeccion<T>(string nombreColeccion)
        {
            return _database.GetCollection<T>(nombreColeccion);
        }
        public async Task<TrampasPaginasDto> GetTrampasUsuarioPaginado(UsuarioYPaginadoDto usuarioYPaginadoDto)
        {
            const int trampasPorPagina = 16;
            IMongoCollection<TrampaModel> collection = ObtenerColeccion<TrampaModel>("Trampa");

            try
            {
                // Filtro para el ID de usuario (asumiendo que TrampaModel tiene una propiedad UsuarioId)
                var filtroUsuario = Builders<TrampaModel>.Filter.Eq(t => t.IDUsuario, usuarioYPaginadoDto.UsuarioId);

                // Contar solo los registros del usuario
                var totalRegistros = await collection.CountDocumentsAsync(filtroUsuario);
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / trampasPorPagina);

                // Obtener las trampas solo del usuario, con paginación
                var trampas = await collection.Find(filtroUsuario)
                    .Skip((usuarioYPaginadoDto.Pagina - 1) * trampasPorPagina)
                    .Limit(trampasPorPagina)
                    .ToListAsync();

                return new TrampasPaginasDto(trampas: trampas, totalRegistros: totalRegistros, totalPaginas: totalPaginas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener trampas: {ex.Message}");
                return new TrampasPaginasDto([], 0, 0);
            }
        }

        public async Task<TrampasPaginasDto> EncontrarTodasTrampasPaginado(int pagina = 1)
        {
            const int trampasPorPagina = 16;
            IMongoCollection<TrampaModel> collection = ObtenerColeccion<TrampaModel>("Trampa");
            
            try
            {
                var totalRegistros = await collection.CountDocumentsAsync(_ => true);
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / trampasPorPagina);
                
                var trampas = await collection.Find(_ => true)
                    .Skip((pagina - 1) * trampasPorPagina)
                    .Limit(trampasPorPagina)
                    .ToListAsync();
                    
                return new TrampasPaginasDto(trampas: trampas, totalRegistros: totalRegistros, totalPaginas: totalPaginas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener trampas: {ex.Message}");
                return new TrampasPaginasDto([], 0, 0);
            }
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

        public async Task<EstadisticasCapturasDto> MostrarEstadistica(int trampaId)
        {
            var capturasCollection = ObtenerColeccion("Captura");
            var estadisticas = new EstadisticasCapturasDto();

            try
            {
                var pipeline = new[]
                {
                    new BsonDocument("$match",
                        new BsonDocument("IDTrampa", trampaId)),
                    new BsonDocument("$sort",
                        new BsonDocument("FechaCaptura", 1)),
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "FechaExacta", "$FechaCaptura" },
                            { "Dia", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m-%d" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Semana", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%G-Semana%V" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Mes", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Año", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            }
                        })
                };

                var resultados = await capturasCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                foreach (var resultado in resultados)
                {
                    var fechaExacta = resultado["FechaExacta"].ToUniversalTime();
                    var dia = resultado["Dia"].AsString;
                    var semana = resultado["Semana"].AsString;
                    var mes = resultado["Mes"].AsString;
                    var año = resultado["Año"].AsString;

                    // Agregar la fecha exacta (con hora) a la lista de capturas por día
                    if (!estadisticas.CapturasPorDia.ContainsKey(dia))
                    {
                        estadisticas.CapturasPorDia[dia] = new List<DateTime>();
                    }
                    estadisticas.CapturasPorDia[dia].Add(fechaExacta);

                    // Mantener el conteo para semana, mes y año
                    estadisticas.CapturasPorSemana[semana] = estadisticas.CapturasPorSemana.ContainsKey(semana) ?
                        estadisticas.CapturasPorSemana[semana] + 1 : 1;

                    estadisticas.CapturasPorMes[mes] = estadisticas.CapturasPorMes.ContainsKey(mes) ?
                        estadisticas.CapturasPorMes[mes] + 1 : 1;

                    estadisticas.CapturasPorAño[año] = estadisticas.CapturasPorAño.ContainsKey(año) ?
                        estadisticas.CapturasPorAño[año] + 1 : 1;
                }

                return estadisticas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MostrarEstadistica: {ex.Message}");
                return estadisticas;
            }
        }

        public async Task<EstadisticasCapturasDto> MostrarEstadisticaGeneral()
        {
            var capturasCollection = ObtenerColeccion("Captura");
            var estadisticas = new EstadisticasCapturasDto();

            try
            {
                var pipeline = new[]
                {
                    new BsonDocument("$sort",
                        new BsonDocument("FechaCaptura", 1)),
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "FechaExacta", "$FechaCaptura" },
                            { "Dia", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m-%d" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Semana", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%G-Semana%V" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Mes", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Año", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            }
                        })
                };

                var resultados = await capturasCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                foreach (var resultado in resultados)
                {
                    var fechaExacta = resultado["FechaExacta"].ToUniversalTime();
                    var dia = resultado["Dia"].AsString;
                    var semana = resultado["Semana"].AsString;
                    var mes = resultado["Mes"].AsString;
                    var año = resultado["Año"].AsString;

                    // Agregar la fecha exacta (con hora) a la lista de capturas por día
                    if (!estadisticas.CapturasPorDia.ContainsKey(dia))
                    {
                        estadisticas.CapturasPorDia[dia] = new List<DateTime>();
                    }
                    estadisticas.CapturasPorDia[dia].Add(fechaExacta);

                    // Mantener el conteo para semana, mes y año
                    estadisticas.CapturasPorSemana[semana] = estadisticas.CapturasPorSemana.ContainsKey(semana) ?
                        estadisticas.CapturasPorSemana[semana] + 1 : 1;

                    estadisticas.CapturasPorMes[mes] = estadisticas.CapturasPorMes.ContainsKey(mes) ?
                        estadisticas.CapturasPorMes[mes] + 1 : 1;

                    estadisticas.CapturasPorAño[año] = estadisticas.CapturasPorAño.ContainsKey(año) ?
                        estadisticas.CapturasPorAño[año] + 1 : 1;
                }

                return estadisticas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MostrarEstadisticaGeneral: {ex.Message}");
                return estadisticas;
            }
        }

        public async Task<EstadisticasCapturasDto> MostrarEstadisticaUsuario(int userId)
        {
            var capturasCollection = ObtenerColeccion("Captura");
            var trampasCollection = ObtenerColeccion("Trampa");
            var estadisticas = new EstadisticasCapturasDto();

            try
            {
                var trampasUsuario = await trampasCollection
                    .Find(Builders<BsonDocument>.Filter.Eq("IDUsuario", userId))
                    .Project(Builders<BsonDocument>.Projection.Include("IDTrampa"))
                    .ToListAsync();

                if (trampasUsuario.Count == 0)
                    return estadisticas;

                var trampaIds = trampasUsuario.Select(t => t["IDTrampa"].AsInt32).ToList();

                var pipeline = new[]
                {
                    new BsonDocument("$match",
                        new BsonDocument("IDTrampa",
                            new BsonDocument("$in", new BsonArray(trampaIds)))),
                    new BsonDocument("$sort",
                        new BsonDocument("FechaCaptura", 1)),
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "FechaExacta", "$FechaCaptura" },
                            { "Dia", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m-%d" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Semana", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%G-Semana%V" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Mes", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y-%m" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            },
                            { "Año", new BsonDocument("$dateToString",
                                new BsonDocument
                                {
                                    { "format", "%Y" },
                                    { "date", "$FechaCaptura" },
                                    { "timezone", "UTC" }
                                })
                            }
                        })
                };

                var resultados = await capturasCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                foreach (var resultado in resultados)
                {
                    var fechaExacta = resultado["FechaExacta"].ToUniversalTime();
                    var dia = resultado["Dia"].AsString;
                    var semana = resultado["Semana"].AsString;
                    var mes = resultado["Mes"].AsString;
                    var año = resultado["Año"].AsString;

                    // Agregar la fecha exacta (con hora) a la lista de capturas por día
                    if (!estadisticas.CapturasPorDia.ContainsKey(dia))
                    {
                        estadisticas.CapturasPorDia[dia] = new List<DateTime>();
                    }
                    estadisticas.CapturasPorDia[dia].Add(fechaExacta);

                    // Mantener el conteo para semana, mes y año
                    estadisticas.CapturasPorSemana[semana] = estadisticas.CapturasPorSemana.ContainsKey(semana) ?
                        estadisticas.CapturasPorSemana[semana] + 1 : 1;

                    estadisticas.CapturasPorMes[mes] = estadisticas.CapturasPorMes.ContainsKey(mes) ?
                        estadisticas.CapturasPorMes[mes] + 1 : 1;

                    estadisticas.CapturasPorAño[año] = estadisticas.CapturasPorAño.ContainsKey(año) ?
                        estadisticas.CapturasPorAño[año] + 1 : 1;
                }

                return estadisticas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MostrarEstadisticaUsuario: {ex.Message}");
                return estadisticas;
            }
        }

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
                    // Deserializar el documento actualizado a TrampaModel
                    var trampaActualizada = BsonSerializer.Deserialize<TrampaModel>(documentoActualizado);

                    // Notificar a todos los clientes conectados sobre la actualización
                    await _hubContext.Clients.All.SendAsync("ActualizarTrampas", trampaActualizada.IDUsuario);

                    return trampaActualizada;
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
        // Filtro combinado: IDTrampa coincide y IDUsuario es exactamente 0
        var filtro = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("IDTrampa", trampaID),
            Builders<BsonDocument>.Filter.Eq("IDUsuario", 0)
        );

        var documento = await collection.Find(filtro).FirstOrDefaultAsync();

        if (documento != null)
        {
            Console.WriteLine($"Documento encontrado: {documento.ToJson()}"); // Depuración
            return BsonSerializer.Deserialize<TrampaModel>(documento);
        }

        Console.WriteLine($"No se encontró trampa con IDTrampa: {trampaID} y IDUsuario = 0."); // Depuración
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en BuscarTrampa: {ex.Message}"); // Depuración
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
                { "Modelo", agregartrampa.Modelo },
                { "Localizacion", BsonNull.Value },
                { "EstatusTrampa", BsonNull.Value },
                { "EstatusPuerta", BsonNull.Value },
                { "EstatusSensor", BsonNull.Value }

            };

            await ObtenerColeccion("Trampa").InsertOneAsync(documento);

            return 
            new TrampaModel
            {
                IDTrampa = documento.GetValue("IDTrampa").ToInt32(),
                Modelo = documento.GetValue("Modelo").ToString(),
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

        #region ObtenerEstatusSensor
        public async Task<bool?> ObtenerEstatusSensor(int trampaID)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", trampaID);
                var proyeccion = Builders<BsonDocument>.Projection
                    .Include("EstatusSensor")
                    .Exclude("_id");

                var documento = await collection.Find(filtro)
                    .Project(proyeccion)
                    .FirstOrDefaultAsync();

                if (documento != null)
                {
                    return documento["EstatusSensor"].AsBoolean;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region ObtenerEstatusPuerta
        public async Task<bool?> ObtenerEstatusPuerta(int trampaID)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", trampaID);
                var proyeccion = Builders<BsonDocument>.Projection
                    .Include("EstatusPuerta")
                    .Exclude("_id");

                var documento = await collection.Find(filtro)
                    .Project(proyeccion)
                    .FirstOrDefaultAsync();

                if (documento != null)
                {
                    return documento["EstatusPuerta"].AsBoolean;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
