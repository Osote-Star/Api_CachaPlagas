using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CapturaDto
{
    public class CapturaDto
    {
        public int IDCaptura { get; set; }
        public int? IDTrampa { get; set; }
        public string Animal { get; set; } = "desconocido";
        public DateTime? FechaCaptura { get; set; }
    }
}
