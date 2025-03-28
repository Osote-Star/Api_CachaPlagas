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

namespace Data.Services
{
    public class ServiceEMail : IservicioEmail
    {
        private readonly IConfiguration configuration;


        public ServiceEMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task EnviarEmail(string emailReceptor, string tema, string cuerpo)
        {
            var emailEmisor = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            var puerto = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");

            var smtpClient = new SmtpClient(host, puerto);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(emailEmisor, password);

            var mensaje = new MailMessage(emailEmisor!, emailReceptor, tema, cuerpo);
            await smtpClient.SendMailAsync(mensaje);
            
        }
    }
}
