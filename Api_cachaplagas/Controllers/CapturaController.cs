using Data.Interfaces;
using DTOs.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CapturaController : ControllerBase
    {
        private ICapturaService _services;
        public CapturaController(ICapturaService services) => _services = services;

        // POST api/<CapturaController>
        [HttpPost("AgregarCaptura/{TrampaId}")]
        [Authorize(AuthenticationSchemes = "TokenUsuario,TokenTrampa")]
        public async Task<IActionResult> AgregarCaptura(int TrampaId)
        {
            CapturaModel captura = await _services.AgregarCaptura(TrampaId);
            if (captura == null)
            {
                return NotFound();
            }
            return Ok(captura);
        }

        
    }
}
