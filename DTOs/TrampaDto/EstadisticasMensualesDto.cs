using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class EstadisticasMensualesDto
    {
        public int[] CapturasPorMes { get; set; } = new int[12];
        public int Año { get; set; }
    }
}
