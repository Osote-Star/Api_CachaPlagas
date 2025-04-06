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
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("ObtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerUsuarios()
        {
            var usuarios = await _services.ConsultarUsuario();
            return Ok(usuarios);
        }

        // POST api/<UsuariosController>
<<<<<<< HEAD
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto createUserDto)
        {
            UsuariosModel? task = await _services.AgregarUsuario(createUserDto);
            if (task == null) return NotFound();
            return Ok(task);
=======
        [HttpPost("AgregarUsuario")]
        public async Task<IActionResult> AgregarUsuario([FromBody] CreateUserDto createUserDto)
        {
            UsuariosModel user = await _services.AgregarUsuario(createUserDto);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
        }

        // PUT api/<UsuariosController>/5
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("CambiarContrasena")]
        public async Task<IActionResult> Put([FromBody] CambiarContrasenaDto cambiarContrasenaDto)
        {
<<<<<<< HEAD
            UsuariosModel? task = await _services.RecuperarContrasena(recuperarContrasenaDto);
=======
            UsuariosModel task = await _services.CambiarContrasena(cambiarContrasenaDto);
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
            if (task == null) return NotFound();
            return Ok(task);

        }
    }
}
