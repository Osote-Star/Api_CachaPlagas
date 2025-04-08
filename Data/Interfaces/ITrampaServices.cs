using DTOs.TrampaDto;
using Models;
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
        public Task<TrampaModel> CambiarStatusTrampa(CambiarStatusDto cambiarStatusDto);
        public Task<TrampaModel> CambiarStatusSensor(EstatusSensorDto estatusSensor);
        public Task<TrampaModel> CambiarEstatusPuerta(EstatusPuertaDto estatusPuertaDto);
        public Task<List<TrampaModel>> TodasTrampas(int usuarioID);
        public Task<TrampaModel> BuscarTrampa(int trampaID);
        public Task<TrampaModel> AgregarTrampa(AgregarTrampaDto agregartrampa);
        public Task<bool> EditarLocalizacion(EditarLocalizacionDto dto);
        public Task<bool?> ObtenerEstatusSensor(int trampaID);
        public Task<bool?> ObtenerEstatusPuerta(int trampaID);
        public Task<TrampasPaginasDto> EncontrarTodasTrampasPaginado(int pagina = 1);
        public Task<TrampasPaginasDto> GetTrampasUsuarioPaginado(UsuarioYPaginadoDto usuarioYPaginadoDto);
        public Task<IEnumerable<TrampaModel>> MostrarEstadisticaGeneral();
        public Task<IEnumerable<TrampaModel>> MostrarEstadisticaUsuario(int userId);
        public Task<TrampaModel> MostrarEstadisticaModelo(string modelo);
        public Task<int> GetTrampasCount();
        public Task<TrampasPaginasDto> FilterByModel(ModeloYPaginadoDto modeloYPaginadoDto);
        public Task<TrampaModel> EditarTrampa(EditarTrampaDto editarTrampaDto);

    }
}
