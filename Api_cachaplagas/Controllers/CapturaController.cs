using Data.Interfaces;
using DTOs.CapturaDto;
using DTOs.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapturaController : ControllerBase
    {
        private ICapturaService _services;
        public CapturaController(ICapturaService services) => _services = services;


        // POST api/<CapturaController>
        [HttpPost("AgregarCaptura")]
        public async Task<IActionResult> AgregarCaptura([FromBody] AgregarCapturaDto agregarCapturaDto)
        {
            CapturaModel captura = await _services.AgregarCaptura(agregarCapturaDto);
            if (captura == null)
            {
                return NotFound();
            }
            return Ok(captura);
        }

        
    }
}
