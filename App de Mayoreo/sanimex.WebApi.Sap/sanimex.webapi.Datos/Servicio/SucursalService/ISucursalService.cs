using sanimex.webapi.Dominio.Models.Producto;
using sanimex.webapi.Dominio.Models.Sucursales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.SucursalService
{
    public interface ISucursalService
    {
        Task<List<Sucursal>> ObtenerSucursalSupervisor(int idUsuario);
        Task<List<Sucursal>> ObtenerSucursales(int idUsuario);
        Task<List<Sucursal>> ObtenerSucursalesAdmin();
        Task<TipoVenta> ObtenerTipoVenta(string ClaveSap);
    }
}
