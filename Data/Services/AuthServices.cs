using Data.Interfaces;
using DTOs.UsuariosDto;
using Microsoft.IdentityModel.Tokens;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;   

namespace Data.Services
{
    public class AuthServices : IAuthServices
    {
        private IMongoDatabase _database;

        public AuthServices(MongoConfiguration client) => _database = client.GetClient().GetDatabase("CachaPlagas");

        public IMongoCollection<BsonDocument> ObtenerColeccion(string nombreColeccion)
        {
            return _database.GetCollection<BsonDocument>(nombreColeccion);
        }
        public Task<UsuariosModel> RecuperarContrasena(string Contrasena)
        {
            throw new NotImplementedException();
        }
        public async Task<string> Login(LoginDto loginDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("Email",loginDto.Email);

                var documento = await collection.Find(filtro).FirstOrDefaultAsync();
                if (documento == null) return "";

                var user = BsonSerializer.Deserialize<UsuariosModel>(documento);
                bool ContraseñaCorrespondida = BC.EnhancedVerify(loginDto.Contrasena, user.Contrasena);

                if (!ContraseñaCorrespondida) return "";
               
                return GenerarToken(user);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GenerarToken(UsuariosModel usuario)
        {
            var userClaim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString() ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol ?? ""),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: userClaim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
