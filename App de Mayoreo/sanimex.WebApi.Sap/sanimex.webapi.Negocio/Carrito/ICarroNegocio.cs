using sanimex.webapi.Dominio.Models.Carrito;

namespace sanimex.webapi.Negocio.Carrito
{
    public interface ICarroNegocio
    {
        Task<HisCtoCliente?> ConsultaCliente(string idUsuario, string idCotizacion);
        Task<List<CotizacionDetalle>> CotizacionDetalle(string idUsuario, string idCotizacion);
        Task<bool> GuardarCodigoAsync(int idGenerado, List<Carro> carro);
        Task<int> GuardarConsultas(string idUsuario, List<Carro> carro);
        Task<List<HisCotizaGerenteSucursal>> HisCotizaGerenteSucursal(string idUsuario, string fechaConsulta);
        Task<List<HisCtoMaster>> ListarHistorialCMaster(string idUsuario, string fechaConsulta);
        Task<List<HisCtoMasterGerente>> ListarHistorialCMasterGerente(string idUsuario, string fechaConsulta);
    }
}