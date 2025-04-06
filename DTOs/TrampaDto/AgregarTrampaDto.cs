using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class AgregarTrampaDto
    {
        public int IDTrampa { get; set; }
        public string Imagen { get; set; }
        public string Modelo { get; set; }
        public bool EstatusTrampa { get; set; }
        public bool EstatusPuerta { get; set; }
        public bool EstatusSensor { get; set; }
    }
}
