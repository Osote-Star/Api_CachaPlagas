using DTOs.CapturaDto;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICapturaService
    {
        public Task<CapturaModel> AgregarCaptura(AgregarCapturaDto agregarCapturaDto);
    }
}
