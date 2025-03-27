using Data.Interfaces;
using DTOs.TrampaDto;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrampaController : ControllerBase
    {
        private ITrampaServices _services;
        public TrampaController(ITrampaServices services) => _services = services;


        // GET: api/<TrampaController>
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
        [HttpGet("MostrarEstadistica")]
        public async Task<IActionResult> Get(int TrampaID)
        {
            TrampaModel task = await _services.MostrarEstadistica(TrampaID);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST api/<TrampaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TrampaController>/5
        [HttpPut("AnadirTrampa")]
        public async Task<IActionResult> Put([FromBody] VincularTrampaDto vincularTrampaDto)
        {
            TrampaModel trampa = await _services.VincularTrampa(vincularTrampaDto);
            if(trampa == null) return NotFound();
            return Ok(trampa);  
        }

        // DELETE api/<TrampaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
