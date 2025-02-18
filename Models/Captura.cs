using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Captura
    {
        int IDCaptura { get; set; }
        int IDTrampa { get; set; }
        string Animal { get; set; }
        DateTime FechaCaptura { get; set; }
    }
}
