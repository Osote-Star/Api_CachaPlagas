using Data.Interfaces;
using DTOs.TrampaDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Formats.Asn1;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrampaController : ControllerBase
    {
        private ITrampaServices _services;
        public TrampaController(ITrampaServices services) => _services = services;


        // GET: api/<TrampaController>
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("Buscar-las-trampas-del-usuario/{usuarioID}")]
        public async Task<ActionResult<List<TrampaModel>>> GetTodasTrampas(int usuarioID)
        {
            var trampas = await _services.TodasTrampas(usuarioID);
            if (trampas == null || trampas.Count == 0)
            {
                return NotFound("No se encontraron trampas para el usuario especificado.");
            }
            return Ok(trampas);
        }

        // GET api/<TrampaController>/5
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("Buscar-trampa/{trampaID}")]
        public async Task<ActionResult<TrampaModel>> GetTrampas(int trampaID)
        {
            var trampa = await _services.BuscarTrampa(trampaID);
            if (trampa == null)
            {
                return NotFound("No se encontró la trampa especificada.");
            }
            return Ok(trampa);
        }

        // GET api/<TrampaController>/5
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarEstadistica")]
        public async Task<IActionResult> MostrarEstadistica(int TrampaID)
        {
            TrampaModel task = await _services.MostrarEstadistica(TrampaID);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario,TokenTrampa")]
        [HttpGet("ObtenerEstatusSensor/{trampaID}")]
        public async Task<IActionResult> GetEstatusSensor(int trampaID)
        {
            var estatus = await _services.ObtenerEstatusSensor(trampaID);
            if (estatus == null)
            {
                return NotFound("No se encontró la trampa especificada o su estatus de sensor.");
            }
            return Ok(new { EstatusSensor = estatus });
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario,TokenTrampa")]
        [HttpGet("ObtenerEstatusPuerta/{trampaID}")]
        public async Task<IActionResult> GetEstatusPuerta(int trampaID)
        {
            var estatus = await _services.ObtenerEstatusPuerta(trampaID);
            if (estatus == null)
            {
                return NotFound("No se encontró la trampa especificada o su estatus de puerta.");
            }
            return Ok(new { EstatusPuerta = estatus });
        }

        // POST api/<TrampaController>
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPost("AgregarTrampa")]
        public async Task<ActionResult<TrampaModel>> AgregarTrampa([FromBody] AgregarTrampaDto agregarTrampa)
        {
            var trampaCreada = await _services.AgregarTrampa(agregarTrampa);
            return Created("/api/Trampa/" + trampaCreada.IDTrampa, trampaCreada);
        }

        // PUT api/<TrampaController>/5
<<<<<<< HEAD
        [HttpPut("VincularTrampa")]
        public async Task<IActionResult> VincularTrampa([FromBody] VincularTrampaDto vincularTrampaDto)
=======
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("VincularTrampa")]
        public async Task<IActionResult> Put([FromBody] VincularTrampaDto vincularTrampaDto)
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
        {
            TrampaModel trampa = await _services.VincularTrampa(vincularTrampaDto);
            if(trampa == null) return NotFound();
            return Ok(trampa);  
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("CambiarStatusTrampa")]
        public async Task<IActionResult> Put([FromBody] CambiarStatusDto cambiarStatusDto)
        {
            TrampaModel trampa = await _services.CambiarStatusTrampa(cambiarStatusDto);
            if (trampa == null) return NotFound();
            return Ok(trampa);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("CambiarestatusSensor")]
        public async Task<IActionResult> CambiarStatusSensor([FromBody] EstatusSensorDto estatusSensor)
        {
            TrampaModel trampa = await _services.CambiarStatusSensor(estatusSensor);
            if (trampa == null) return NotFound();
            return Ok(trampa);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario,TokenTrampa")]
        [HttpPut("CambiarEstatusPuerta")]
        public async Task<IActionResult> CambiarEstatusPuerta([FromBody] EstatusPuertaDto estatusPuertaDto)
        {
            TrampaModel trampa = await _services.CambiarEstatusPuerta(estatusPuertaDto);
            if (trampa == null) return NotFound();
            return Ok(trampa);
        }

        // PUT api/<TrampaController>/5
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("Editar Localizacion")]
        public async Task<IActionResult> EditarSitio([FromBody] EditarLocalizacionDto dto)
        {
            var success = await _services.EditarLocalizacion(dto);
            if (!success)
                return NotFound(new { message = "Trampa no encontrada o sin cambios" });

            return Ok(new { message = "Ubicación actualizada correctamente" });
        }



    }
}
