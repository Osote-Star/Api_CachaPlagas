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

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPost("GetAllTrampasUsuario")]
        public async Task<IActionResult> GetAllTrampasUsuario([FromBody] UsuarioYPaginadoDto usuarioYPaginadoDto)
        {
            var trampas = await _services.GetTrampasUsuarioPaginado(usuarioYPaginadoDto);
            if (trampas.TotalRegistros == 0)
                return NotFound(new { message = "Trampa no encontrada o sin cambios" });

            return Ok(trampas);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("GetAllTrampas/{pagina}")]
        public async Task<IActionResult> EncontrarTodasTrampas(int pagina = 1)
        {
            var trampas = await _services.EncontrarTodasTrampasPaginado(pagina = 1);
            if (trampas.TotalRegistros == 0)
                return NotFound(new { message = "Trampa no encontrada o sin cambios" });

            return Ok(new { trampas.Trampas, trampas.TotalRegistros, trampas.TotalPaginas });
        }

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

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPost("EditarTrampa")]
        public async Task<IActionResult> EditarTrampa([FromBody] EditarTrampaDto editarTrampaDto)
        {
            var trampas = await _services.EditarTrampa(editarTrampaDto);
            if (trampas == null)
                return NotFound();

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

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPost("FiltrarPorModelo")]
        public async Task<IActionResult> FilterByModel([FromBody] ModeloYPaginadoDto modeloYPaginadoDto)
        {
            var trampas = await _services.FilterByModel(modeloYPaginadoDto);
            if (trampas.TotalRegistros == 0)
                return NotFound(new { message = "Trampa no encontrada o sin cambios" });
            return Ok(trampas);
        }
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarConteoTrampas")]
        public async Task<IActionResult> GetTrampasCount()
        {
            int task = await _services.GetTrampasCount();
            if (task == 0) return NotFound();
            return Ok(task);
        }

        // GET api/<TrampaController>/5
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarEstadistica/{TrampaID}")]
        public async Task<IActionResult> MostrarEstadistica(int TrampaID)
        {
            var task = await _services.MostrarEstadistica(TrampaID);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarEstadisticaModelo/{modelo}")]
        public async Task<IActionResult> MostrarEstadisticaModelo(string modelo)
        {
            TrampaModel task = await _services.MostrarEstadisticaModelo(modelo);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarEstadisticaGeneral")]
        public async Task<IActionResult> MostrarEstadisticaGeneral()
        {
            var estadisticas = await _services.MostrarEstadisticaGeneral();
            if (estadisticas.CapturasPorDia.Count == 0)
                return NotFound(new { message = "No se encontraron estadísticas generales." });

            return Ok(estadisticas);
        }

        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpGet("MostrarEstadisticaUsuario/{userId}")]
        public async Task<IActionResult> MostrarEstadisticaUsuario(int userId)
        {
            var estadisticas = await _services.MostrarEstadisticaUsuario(userId);
            if (estadisticas.CapturasPorDia.Count == 0)
                return NotFound(new { message = "No se encontraron estadísticas para el usuario especificado." });

            return Ok(estadisticas);
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
        [Authorize(AuthenticationSchemes = "TokenUsuario")]
        [HttpPut("VincularTrampa")]
        public async Task<IActionResult> Put([FromBody] VincularTrampaDto vincularTrampaDto)
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
