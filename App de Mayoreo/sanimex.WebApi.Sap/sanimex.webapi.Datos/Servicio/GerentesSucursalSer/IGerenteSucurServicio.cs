using sanimex.webapi.Dominio.Models.GerentesSucursal;

namespace sanimex.webapi.Datos.Servicio.GerentesSucursalSer
{
    public interface IGerenteSucurServicio
    {
        Task<bool> GuardarSucursalGerente(int idGerente, string Sucursales);
        Task<GerenteSucursal> ObtenerSucursalGerente(int id);
    }
}