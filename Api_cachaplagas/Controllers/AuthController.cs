using Data.Interfaces;
using Data.Services;
using DTOs.AuthDto;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _services.Login(loginDto);
            if (token is null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            try
            {
                var newTokens = await _services.RefreshToken(tokenDto);
                return Ok(newTokens);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("LoginTrampa/{idTrampa}")]
        public async Task<IActionResult> LoginTrampa(int idTrampa)
        {
            var token = await _services.LoginTrampa(idTrampa);
            if (token is null) return Unauthorized();
            return Ok(token);
        }


    }
}
