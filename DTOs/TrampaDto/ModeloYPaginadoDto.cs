using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class ModeloYPaginadoDto
    {
        public string Modelo { get; set; } = string.Empty;
        public int Pagina { get; set; } = 1;
    }
}
