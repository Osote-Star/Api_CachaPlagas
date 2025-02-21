using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioService
    {
        public Task<TrampaModel> VincularTrampa(int trampaID);
    }
}
