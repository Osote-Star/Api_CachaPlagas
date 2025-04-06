using Data.Interfaces;
<<<<<<< HEAD
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Mvc;
=======
using Data.Services;
using DTOs.AuthDto;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
<<<<<<< HEAD
        private IAuthServices _service;
        public AuthController(IAuthServices services) => _service = services;


        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
=======
        private IAuthServices _services;
        public AuthController(IAuthServices services) => _services = services;
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6


        // POST api/<AuthController>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
<<<<<<< HEAD
            var token = await _service.Login(loginDto);
            if (token == "") return Unauthorized();
            return Ok(token);

=======
            var token = await _services.Login(loginDto);
            if (token is null) return Unauthorized();
            return Ok(token);
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
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
