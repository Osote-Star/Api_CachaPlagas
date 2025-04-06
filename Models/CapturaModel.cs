using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CapturaModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _Id { get; set; }
        [BsonElement("IDCaptura")]
        public int? IDCaptura { get; set; }
        [BsonElement("IDTrampa")] 
        public int? IDTrampa { get; set; }
        [BsonElement("Animal")]
<<<<<<< HEAD
        public string? Animal { get; set; }
=======
        public string Animal { get; set; } = "desconocido";
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
        [BsonElement("FechaCaptura")]
        public DateTime? FechaCaptura { get; set; }
    }
}
