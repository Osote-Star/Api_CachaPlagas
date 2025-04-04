using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DTOs.UsuariosDto;
using Models;
using System.Collections.Concurrent;

namespace Data.Services
{
    public class ServiceEMail : IservicioEmail
    {
        private readonly IConfiguration configuration;
        private static readonly ConcurrentDictionary<string, string> codigosValidacion = new();


        public ServiceEMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarEmail(EmailDto emailDto)
        {
            var emailEmisor = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");

            var smtpClient = new SmtpClient(host, puerto);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailEmisor, password);

            string codigoValidacion = new Random().Next(100000, 999999).ToString();
            string cuerpo = $"Su código de validación es: {codigoValidacion}";

            emailDto.tema = "Recuperación De Contraseña";

            // Guardar el código en memoria
            codigosValidacion[emailDto.emailReceptor] = codigoValidacion;

            var mensaje = new MailMessage(emailEmisor!, emailDto.emailReceptor, emailDto.tema, cuerpo);
            await smtpClient.SendMailAsync(mensaje);
            
        }

        public bool ValidarCodigo(ValidarCodigoDto validarCodigoDto)
        {
            if (codigosValidacion.TryGetValue(validarCodigoDto.emailReceptor, out var codigoGuardado))
            {
                if (codigoGuardado == validarCodigoDto.codigo)
                {
                    codigosValidacion.TryRemove(validarCodigoDto.emailReceptor, out _); // Eliminar código después de usarlo
                    return true;
                }
            }
            return false;
        }
    }
}