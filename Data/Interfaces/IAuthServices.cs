using DTOs.AuthDto;
using DTOs.UsuariosDto;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAuthServices
    {
        public Task<UsuariosModel> RecuperarContrasena(string Contrasena);
        public Task<TokenDto> Login(LoginDto loginDto);
        public Task<TokenDto> ReturnsTokens(UsuariosModel usuario);
        public Task<TokenDto> RefreshToken(TokenDto tokenDto);
        public Task<string> LoginTrampa(int idTrampa);

    }
}
