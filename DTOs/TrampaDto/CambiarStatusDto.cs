using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class CambiarStatusDto
    {
        public int IDtrampa { get; set; }
        public bool EstatusTrampa { get; set; }
        public bool EstatusPuerta { get; set; } = false;

        public bool EstatusSensor { get; set; } = false;
    }
}
