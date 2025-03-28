using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UsuariosModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        [BsonElement("IDUsuario")]
        public int IDUsuario {  get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Contrasena")]
        public string Contrasena { get; set; }
        [BsonElement("Rol")]
        public string Rol { get; set; } = "usuario";
        public List<TrampaModel> Trampas { get; set; } = [];
    }
}
