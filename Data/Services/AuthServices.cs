using Data.Interfaces;
using DTOs.AuthDto;
using DTOs.Usuarios;
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
using System.Security.Cryptography;
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
        public IMongoCollection<RefreshTokenModel> ObtenerColeccion<RefreshTokenModel>(string nombreColeccion)
        {
            return _database.GetCollection<RefreshTokenModel>(nombreColeccion);
        }
        public Task<UsuariosModel> RecuperarContrasena(string Contrasena)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenDto> Login(LoginDto loginDto)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Usuario");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("Email", loginDto.Email);

                var documento = await collection.Find(filtro).FirstOrDefaultAsync();
                if (documento == null) return null;

                var user = BsonSerializer.Deserialize<UsuariosModel>(documento);
                bool ContraseñaCorrespondida = BC.EnhancedVerify(loginDto.Contrasena, user.Contrasena);

                if (!ContraseñaCorrespondida) return null;

                return await ReturnsTokens(user);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> LoginTrampa(int idTrampa)
        {
            IMongoCollection<BsonDocument> collection = ObtenerColeccion("Trampa");
            try
            {
                var filtro = Builders<BsonDocument>.Filter.Eq("IDTrampa", idTrampa);

                var documento = await collection.Find(filtro).FirstOrDefaultAsync();
                if (documento == null) return null;

                return GenerarTokenParaDispositivo(idTrampa);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GenerarTokenParaDispositivo(int idTrampa)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,idTrampa.ToString() ?? ""),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY_IOT") ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<TokenDto> ReturnsTokens(UsuariosModel usuario)
        {
            
            var AccessToken = GenerarAccessToken(usuario);
            var RefreshToken = await GenerateRefreshToken(usuario);
     
            return new TokenDto(AccessToken, RefreshToken);
        }

        public string GenerarAccessToken(UsuariosModel usuario)
        {
            var userClaim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString() ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol ?? ""),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: userClaim,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "")),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<string> GenerateRefreshToken(UsuariosModel usuario) 
        {
            var ValidateRt = await ValidateRefreshToken(usuario);
            if (ValidateRt != null) return ValidateRt; // Si ya existe un refresh token activo, devolverlo

            var refreshToken = GenerateRandomNumber();

                var collection = ObtenerColeccion("RefreshToken");
                int rtID = await ObtenerProximoId();

                var rtToken = new RefreshTokenModel
                {
                    _Id = ObjectId.GenerateNewId().ToString(),
                    RefreshTokenId = rtID,
                    RefreshToken = refreshToken,
                    IDUsuario = usuario.IDUsuario,
                };

                var bsonUsuario = rtToken.ToBsonDocument();
                await collection.InsertOneAsync(bsonUsuario);

            return refreshToken;
        }
        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var collection = ObtenerColeccion("Usuario");

            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var oldRefreshToken = await RevokeRefreshToken(tokenDto.RefreshToken, userId);

            var filter = Builders<BsonDocument>.Filter.Eq("IDUsuario", userId);
            var usuarioEncontrado = await collection.Find(filter).FirstOrDefaultAsync();
            UsuariosModel usuario = BsonSerializer.Deserialize<UsuariosModel>(usuarioEncontrado);

            return await ReturnsTokens(usuario);
        }            

        private async Task<RefreshTokenModel> RevokeRefreshToken(string rtoken, int userId)
        {
            var collection = ObtenerColeccion<RefreshTokenModel>("RefreshToken");
            var filter = Builders<RefreshTokenModel>.Filter.And(
                Builders<RefreshTokenModel>.Filter.Eq(rt => rt.RefreshToken, rtoken),
                Builders<RefreshTokenModel>.Filter.Eq(rt => rt.IDUsuario, userId),
                Builders<RefreshTokenModel>.Filter.Eq(rt => rt.IsRevoked, false)
            );

            var refreshToken = await collection.Find(filter).FirstOrDefaultAsync()
                ?? throw new SecurityTokenException("Refresh token inválido");

            if(refreshToken is null || refreshToken.RefreshToken != rtoken || refreshToken.ExpiresAt >= DateTime.UtcNow)
                return null;

            refreshToken.IsRevoked = true;
            await collection.ReplaceOneAsync(filter, refreshToken);

            return refreshToken;
        }
        public async Task<string> ValidateRefreshToken(UsuariosModel usuario) 
        {
            var collection = ObtenerColeccion<RefreshTokenModel>("RefreshToken");

            // 1. Primero verificar si ya existe un refresh token activo
            var filterActivos = Builders<RefreshTokenModel>.Filter.And(
                Builders<RefreshTokenModel>.Filter.Eq(rt => rt.IDUsuario, usuario.IDUsuario),
                Builders<RefreshTokenModel>.Filter.Eq(rt => rt.IsRevoked, false)
            );

            var tokenActivo = await collection.Find(filterActivos).FirstOrDefaultAsync();

            // 2. Si existe uno activo, devolver ese mismo
            if (tokenActivo != null)
            {
                return tokenActivo.RefreshToken;
            }

            return null;
        }
        public async Task<int> ObtenerProximoId()
        {
            IMongoCollection<RefreshTokenModel> documento = ObtenerColeccion<RefreshTokenModel>("RefreshToken");

            // Obtener todos los usuarios y extraer el _Id máximo
            var refreshTokenModel = await documento.Find(_ => true)
                                           .Sort(Builders<RefreshTokenModel>.Sort.Descending(rt => rt.RefreshTokenId))
                                           .Limit(1)
                                           .ToListAsync();


            if (refreshTokenModel.Any())
            {
                return refreshTokenModel.First().RefreshTokenId + 1; // Retornar el siguiente ID disponible
            }

            return 1; // Si no hay usuarios, comenzar desde 1
        }
        private string GenerateRandomNumber()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }    
}
