using sanimex.webapi.Dominio.Models.Carrito;

namespace sanimex.webapi.Datos.Servicio.CarritoServico
{
    public interface ICarritoServicio
    {
        Task<ConsultaCliente?> ConsultaCliente(string idCotizacion);
        Task<List<CotizacionDetalle>> CotizacionDetalle(string idUsuario, string idCotizacion);
        Task<int> GuardarCarro(string idUsuario, List<Carro> carros);
        Task<bool> GuardarCodigoAsync(int idGenerado, List<Carro> carros);
        Task<List<HisCotizaGerenteSucursal>> HisCotizaGerenteSucursal(string idUsuario, string fechaConsulta);
        Task<List<HisCtoMaster>> ListarHistorialCMaster(string idUsuario, string fechaConsulta);
        Task<List<HisCtoMasterGerente>> ListarHistorialCMasterGerente(string idUsuario, string fechaConsulta);
        Task<int> ObtenerIdSucursal(string claveSap);
        Task<int> ObtenerNumEmpleado(int idUsuario);
    }
}