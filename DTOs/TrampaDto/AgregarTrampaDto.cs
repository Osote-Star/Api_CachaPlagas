using MongoDB.Bson.Serialization.Attributes;
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
        public int IDUsuario { get; set; }
        public string Imagen { get; set; }
        public string Modelo { get; set; }
        public string Localizacion { get; set; }
        public string EstatusTrampa { get; set; }
        public string EstatusPuerta { get; set; }
        public string EstatusSensor { get; set; }
    }
}
