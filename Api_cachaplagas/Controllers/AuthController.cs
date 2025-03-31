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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _services.Login(loginDto);
            if (token == "") return Unauthorized();
            return Ok(token);

        }


    }
}
