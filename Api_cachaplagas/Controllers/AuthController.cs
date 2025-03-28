using Data.Interfaces;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthServices _services;
        public AuthController(IAuthServices services) => _services = services;


        // POST api/<AuthController>
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Contrasena))
            {
                return BadRequest("Por favor, proporciona un email y una contraseña válidos.");
            }

            var usuario = await _services.Login(loginDto.Email, loginDto.Contrasena);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado o credenciales incorrectas.");
            }

            return Ok(usuario);
        }

    }
}
