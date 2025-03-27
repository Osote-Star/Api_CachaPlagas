using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Mvc;
using Models;
using Data.Interfaces; 




// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IAuthServices _services;
        public AuthController(IAuthServices services) => _services = services;

        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDTO loginDto)
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

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
