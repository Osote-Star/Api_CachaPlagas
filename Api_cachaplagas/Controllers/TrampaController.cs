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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
