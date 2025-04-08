using Data.Interfaces;
using Data.Services;
using DTOs.CapturaDto;
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

        [HttpGet("ObtenerCapturas/{usuarioId}")]
        [Authorize(AuthenticationSchemes = "TokenUsuario,TokenTrampa")]
        public async Task<ActionResult<List<CapturaDto>>> ObtenerCapturasPorUsuario(int usuarioId)
        {
            var capturas = await _services.ObtenerCapturasPorUsuario(usuarioId);
            if (capturas == null || !capturas.Any())
            {
                return NotFound("No se encontraron capturas para este usuario.");
            }

            return Ok(capturas);
        }


    }
}
