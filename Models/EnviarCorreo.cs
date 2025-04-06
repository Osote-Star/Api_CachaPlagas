using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EnviarCorreo
    {
        public string correoReceptor { get; set; }
        public string tema { get; set; } = "Recuperacion de contraseña";
        public string cuerpo { get; set; } = "Hola, hemos recibido un correo de tu parte el cual es para recuperar contraseña";
    }
}
