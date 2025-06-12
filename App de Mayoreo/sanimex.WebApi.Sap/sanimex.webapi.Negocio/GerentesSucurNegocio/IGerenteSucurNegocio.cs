using sanimex.webapi.Dominio.Models.GerentesSucursal;

namespace sanimex.webapi.Negocio.GerentesSucurNegocio
{
    public interface IGerenteSucurNegocio
    {
        Task<GerenteSucursal> GerenteSucursal(int id);
        Task<bool> GuardarSucursalGerente(int idGerente, string Sucursales);
    }
}