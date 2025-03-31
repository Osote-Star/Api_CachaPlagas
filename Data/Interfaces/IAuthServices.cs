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
        public Task<UsuariosModel> CambiarContrasena(string Contrasena);
        public Task<string> Login(LoginDto loginDto);
        public string GenerarToken(UsuariosModel usuario);
    }
}
