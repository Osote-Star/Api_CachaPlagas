using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Usuarios
{
    public class CreateUserDto
    {

        string Nombre { get; set; }
        string Contrasena { get; set; }
        string Email { get; set; }
    }
}
