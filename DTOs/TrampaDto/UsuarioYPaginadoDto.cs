using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class UsuarioYPaginadoDto
    {
        public int Pagina { get; set; } = 1;
        public int UsuarioId { get; set; }
    }
}
