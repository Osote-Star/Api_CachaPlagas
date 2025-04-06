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
        public async Task<IActionResult> Enviar([FromBody] EmailDto emailDto){
            if(emailDto == null || string.IsNullOrEmpty(emailDto.emailReceptor)){
                return BadRequest("El email receptor es requerido");
            }

            await this.servicioEmail.EnviarEmail(emailDto);
            return Ok("Correo Enviado exitosamente");
        }

        [HttpPost("validar")]
        public IActionResult ValidarCodigo([FromBody] ValidarCodigoDto validarCodigoDto)
        {
            if (validarCodigoDto == null || string.IsNullOrEmpty(validarCodigoDto.emailReceptor) || string.IsNullOrEmpty(validarCodigoDto.codigo))
            {
            return BadRequest("El correo y el código son obligatorios.");
            }

            bool esValido = servicioEmail.ValidarCodigo(validarCodigoDto);
            if (esValido)
            {
            return Ok("Código válido.");
            }
            else
            {
            return BadRequest("Código incorrecto o expirado.");
            }
        }
    }
}
