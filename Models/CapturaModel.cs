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
        public string _Id { get; set; }
        [BsonElement("IDCaptura")]
        int IDCaptura { get; set; }
        [BsonElement("IDTrampa")] 
        int IDTrampa { get; set; }
        [BsonElement("Animal")]
        string Animal { get; set; }
        [BsonElement("FechaCaptura")]
        DateTime FechaCaptura { get; set; }
    }
}
