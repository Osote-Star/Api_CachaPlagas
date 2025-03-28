using DTOs.UsuariosDto;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IservicioEmail
    {
        public Task EnviarEmail(string emailReceptor, string tema, string cuerpo);
    }
}
