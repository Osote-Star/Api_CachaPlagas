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
        public string? _Id { get; set; }
        [BsonElement("IDUsuario")]
        public int? IDUsuario {  get; set; }
        [BsonElement("Email")]
        public string? Email { get; set; }
        [BsonElement("Contrasena")]
        public string? Contrasena { get; set; }
        [BsonElement("Rol")]
<<<<<<< HEAD
        public string? Rol { get; set; }
        public List<TrampaModel> Trampas { get; set; } = [];

=======
        public string Rol { get; set; } = "usuario";
        public List<TrampaModel> Trampas { get; set; } = [];
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is UsuariosModel user) return user.IDUsuario == IDUsuario;
            return false;
        }
        public override int GetHashCode()
        {
<<<<<<< HEAD
            return IDUsuario.GetHashCode();
=======
            return Email.GetHashCode();
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
        }
    }
}
