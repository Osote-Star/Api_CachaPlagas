using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_cachaplagas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CapturaController : ControllerBase
    {
        // GET: api/<CapturaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CapturaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CapturaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CapturaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CapturaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
