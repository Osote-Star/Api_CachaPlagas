using Data.Interfaces;
using DTOs.TrampaDto;
using DTOs.UsuariosDto;
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TrampaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TrampaController>
        [HttpPost]
        public async Task<ActionResult<TrampaModel>> AgregarTrampa([FromBody] AgregarTrampaDto agregarTrampa)
        {
            var trampaCreada = await _services.AgregarTrampa(agregarTrampa);
            return Created("/api/Trampa/" + trampaCreada.IDTrampa, trampaCreada);
        }

        // PUT api/<TrampaController>/5
        [HttpPut]
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
