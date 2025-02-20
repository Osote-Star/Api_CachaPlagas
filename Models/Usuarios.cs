using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Usuarios
    {
        int IDUsuario {  get; set; }
        string Email { get; set; }
        string Contrasena { get; set; }
        string Rol { get; set; }
    }
}
