using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RefreshTokenModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        [BsonElement("RefreshTokenId")]
        public int RefreshTokenId { get; set; }  
        [BsonElement("IDUsuario")]
        public int IDUsuario { get; set; }
        [BsonElement("Token")]
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsRevoked { get; set; } = false;
    }
}
