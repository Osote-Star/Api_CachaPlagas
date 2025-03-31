using Data.Interfaces;
using DTOs.Usuarios;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioService _services;
        public UsuariosController(IUsuarioService services) => _services = services;

        // GET: api/<UsuariosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuariosController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto createUserDto)
        {
            UsuariosModel? task = await _services.AgregarUsuario(createUserDto);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("RecuperarContrasena")]
        public async Task<IActionResult> Put([FromBody] RecuperarContrasenaDto recuperarContrasenaDto)
        {
            UsuariosModel? task = await _services.RecuperarContrasena(recuperarContrasenaDto);
            if (task == null) return NotFound();
            return Ok(task);

        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
