using DTOs.TrampaDto;
using DTOs.UsuariosDto;
using Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITrampaServices
    {
        public Task<TrampaModel> VincularTrampa(VincularTrampaDto vincularTrampaDto);
        public Task<TrampaModel> AgregarTrampa(AgregarTrampaDto agregartrampa);
        public Task<bool> EditarLocalizacion(EditarLocalizacionDto dto);


    }
}
