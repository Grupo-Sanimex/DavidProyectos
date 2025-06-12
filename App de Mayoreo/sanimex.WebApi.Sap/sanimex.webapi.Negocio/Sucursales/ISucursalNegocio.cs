using sanimex.webapi.Dominio.Models.Sucursales;

namespace sanimex.webapi.Negocio.Sucursales
{
    public interface ISucursalNegocio
    {
        Task<List<Sucursal>> ObtenerSucursal(int idSucursal);
        Task<List<Sucursal>> ObtenerSucursalAdmin();
        Task<List<Sucursal>> ObtenerSucursalSupervisor(int idUsuario);
    }
}