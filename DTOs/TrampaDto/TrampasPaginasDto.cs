using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TrampaDto
{
    public class TrampasPaginasDto
    {
        public TrampasPaginasDto(IEnumerable<TrampaModel> trampas, long totalRegistros, int totalPaginas)
        {
            Trampas = trampas;
            TotalRegistros = totalRegistros;
            TotalPaginas = totalPaginas;
        }
        public IEnumerable<TrampaModel> Trampas { get; set; } = [];
        public long TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
    }
}
