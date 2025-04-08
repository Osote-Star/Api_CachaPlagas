using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TrampaModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _Id { get; set; }
        [BsonElement("IDTrampa")]
        public int? IDTrampa { get; set; }
        [BsonElement("IDUsuario")]
        public int? IDUsuario { get; set; }
        [BsonElement("Modelo")]
        public string? Modelo { get; set; }
        [BsonElement("Localizacion")]
        public string? Localizacion { get; set; }
        [BsonElement("EstatusTrampa")]
        public bool? EstatusTrampa { get; set; }
        [BsonElement("EstatusSensor")]
        public bool? EstatusSensor { get; set; }
        [BsonElement("EstatusPuerta")]
        public bool? EstatusPuerta { get; set; }
        public List<CapturaModel> Capturas { get; set; } = [];

    }
}
