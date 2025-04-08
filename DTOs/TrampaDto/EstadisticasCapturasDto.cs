using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class EstadisticasCapturasDto
    {
        // Agrupaciones por periodos
        public Dictionary<string, List<DateTime>> CapturasPorDia { get; set; } = new(); // Formato: "YYYY-MM-DD" con lista de horas
        public Dictionary<string, int> CapturasPorSemana { get; set; } = new();         // Formato: "YYYY-SemanaWW"
        public Dictionary<string, int> CapturasPorMes { get; set; } = new();           // Formato: "YYYY-MM"
        public Dictionary<string, int> CapturasPorAño { get; set; } = new();
    }
}
