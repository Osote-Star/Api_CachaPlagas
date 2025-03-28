using Data.Interfaces;
using DTOs.UsuariosDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_cachaplagas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IservicioEmail servicioEmail;

        public EmailsController(IservicioEmail servicioEMail) => this.servicioEmail = servicioEMail;


        [HttpPost]
        public async Task<IActionResult> Enviar(string email, string tema, string cuerpo)
        {
            await servicioEmail.EnviarEmail(email, tema, cuerpo);
            return Ok();
        }
    }
}
