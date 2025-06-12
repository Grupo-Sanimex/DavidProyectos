using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.ControlAcceso
{
    public interface IControlAccesoNegocio
    {
        Task<bool> validarHora();
        Task<bool> validarUltimaHora();
    }
}
