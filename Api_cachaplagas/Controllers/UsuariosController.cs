using Data.Interfaces;
using DTOs.Usuarios;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioService _services;
        public UsuariosController(IUsuarioService services) => _services = services;

        // GET: api/<UsuariosController>
        [HttpGet("ObtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerUsuarios()
        {
            var usuarios = await _services.ConsultarUsuario();
            return Ok(usuarios);
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuariosController>
        [HttpPost("AgregarUsuario")]
        public async Task<IActionResult> AgregarUsuario([FromBody] CreateUserDto createUserDto)
        {
            UsuariosModel user = await _services.AgregarUsuario(createUserDto);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("CambiarContrasena")]
        public async Task<IActionResult> Put([FromBody] CambiarContrasenaDto CambiarContrasenaDto)
        {
            UsuariosModel task = await _services.CambiarContrasena(CambiarContrasenaDto);
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
